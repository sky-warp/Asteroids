using _Project.Scripts.CustomPool;
using _Project.Scripts.Entities;
using _Project.Scripts.LevelBorder;
using R3;
using UnityEngine;

namespace _Project.Scripts.SpawnService
{
    public class ProjectileSpawnService : MonoBehaviour
    {
        [SerializeField] private InputService.InputManager _inputManager;

        [SerializeField] private Projectile _bulletPrefab;
        [SerializeField] private Projectile _laserPrefab;

        [SerializeField] private Transform _shipTransform;
        [SerializeField] private LevelColliderBorder _levelBorder;
        
        private CustomPool<Projectile> _bulletsPool;
        private CustomPool<Projectile> _lasersPool;
        private readonly CompositeDisposable _disposable = new();

        private void Awake()
        {
            _inputManager.OnLeftClick
                .Subscribe(_ => CreateBullet())
                .AddTo(_disposable);
            _inputManager.OnRightClick
                .Subscribe(_ => CreateLaser())
                .AddTo(_disposable);

            _levelBorder.OnProjectileExit
                .Subscribe(DeleteSpawnedProjectile)
                .AddTo(_disposable);

            _bulletsPool = new CustomPool<Projectile>(_bulletPrefab, 3, _shipTransform);
            _lasersPool = new CustomPool<Projectile>(_laserPrefab, 3, _shipTransform);
        }

        private void CreateBullet()
        {
            var bullet = _bulletsPool.Get();
            bullet.MoveProjectile();
        }

        private void CreateLaser()
        {
            var laser = _lasersPool.Get();
            laser.MoveProjectile();
        }

        private void DeleteSpawnedProjectile(Projectile projectile)
        {
            if(projectile.Type == "Bullet")
                _bulletsPool.Release(projectile);
            
            if(projectile.Type == "Laser")
                _lasersPool.Release(projectile);
                
            Destroy(projectile.gameObject);
        }

        private void OnDestroy()
        {
            _disposable.Dispose();
        }
    }
}