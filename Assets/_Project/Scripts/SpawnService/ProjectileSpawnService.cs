using _Project.Scripts.CustomPool;
using _Project.Scripts.LevelBorder;
using _Project.Scripts.Projectiles.ProjectileTypes;
using R3;
using UnityEngine;

namespace _Project.Scripts.SpawnService
{
    public class ProjectileSpawnService : MonoBehaviour
    {
        [Header("InputManager")] [SerializeField]
        private InputService.InputManager _inputManager;

        [Header("Projectiles")] [SerializeField]
        private Bullet _bulletPrefab;

        [SerializeField] private Laser _laserPrefab;

        [Header("Spaceship position")] [SerializeField]
        private Transform _shipTransform;

        [Header("Level's border")] [SerializeField]
        private LevelColliderBorder _levelBorder;

        public Subject<Unit> OnLaserSpawned = new();
        public readonly ReactiveProperty<bool> IsReadyToShootLaser = new();

        private CustomPool<Bullet> _bulletsPool;
        private CustomPool<Laser> _lasersPool;
        private readonly CompositeDisposable _disposable = new();

        private void Awake()
        {
            _inputManager.OnLeftClick
                .Subscribe(_ => CreateBullet())
                .AddTo(_disposable);
            _inputManager.OnRightClick
                .Subscribe(_ => CreateLaser())
                .AddTo(_disposable);

            _levelBorder.OnBulletExit
                .Subscribe(DeleteSpawnedBullet)
                .AddTo(_disposable);
            _levelBorder.OnLaserExit
                .Subscribe(DeleteSpawnedLaser)
                .AddTo(_disposable);

            _bulletsPool = new CustomPool<Bullet>(_bulletPrefab, 3, _shipTransform);
            _lasersPool = new CustomPool<Laser>(_laserPrefab, 3, _shipTransform);
        }

        private void CreateBullet()
        {
            var bullet = _bulletsPool.Get();
            bullet.OnAsteroidHit
                .Subscribe(projectile => DeleteSpawnedBullet((Bullet)projectile))
                .AddTo(_disposable);

            bullet.MoveProjectile();
        }

        public void CreateLaser()
        {
            if (IsReadyToShootLaser.Value)
            {
                var laser = _lasersPool.Get();

                laser.MoveProjectile();
                OnLaserSpawned?.OnNext(Unit.Default);
            }
        }

        private void DeleteSpawnedBullet(Bullet projectile)
        {
            _bulletsPool.Release(projectile);
        }

        private void DeleteSpawnedLaser(Laser projectile)
        {
            _lasersPool.Release(projectile);
        }

        private void OnDestroy()
        {
            _disposable.Dispose();
        }
    }
}