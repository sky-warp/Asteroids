using _Project.Scripts.CustomPool;
using _Project.Scripts.GameOverService;
using _Project.Scripts.InputService;
using _Project.Scripts.LevelBorder;
using _Project.Scripts.Projectiles.ProjectileTypes;
using R3;
using UnityEngine;

namespace _Project.Scripts.SpawnService
{
    public class ProjectileSpawnService
    {
        public readonly Subject<Unit> OnLaserSpawned = new();
        public readonly ReactiveProperty<bool> IsReadyToShootLaser = new();

        private CustomPool<Bullet> _bulletsPool;
        private CustomPool<Laser> _lasersPool;
        private readonly CompositeDisposable _disposable = new();

        private Canvas _levelCanvas;
        
        public ProjectileSpawnService(InputManager inputManager, Bullet bulletPrefab, Laser laserPrefab,
            LevelColliderBorder levelBorder, Transform shipTransform,
            GameOverService.GameOverService gameOverService, Canvas levelCanvas)
        {
            _levelCanvas = levelCanvas;
            
            gameOverService.OnGameOver
                .Subscribe(_ => GameOver())
                .AddTo(_disposable);
            
            inputManager.OnLeftClick
                .Subscribe(_ => CreateBullet())
                .AddTo(_disposable);
            inputManager.OnRightClick
                .Subscribe(_ => CreateLaser())
                .AddTo(_disposable);

            levelBorder.OnBulletExit
                .Subscribe(DeleteSpawnedBullet)
                .AddTo(_disposable);
            levelBorder.OnLaserExit
                .Subscribe(DeleteSpawnedLaser)
                .AddTo(_disposable);

            _bulletsPool = new CustomPool<Bullet>(bulletPrefab, 3, shipTransform);
            _lasersPool = new CustomPool<Laser>(laserPrefab, 3, shipTransform);
        }

        private void GameOver()
        {
            _bulletsPool.ReleaseAll();
            _lasersPool.ReleaseAll();
        }
        
        private void CreateBullet()
        {
            var bullet = _bulletsPool.Get();
            
            bullet.Init(_levelCanvas);
            
            bullet.OnAsteroidHit
                .Subscribe(projectile => DeleteSpawnedBullet((Bullet)projectile))
                .AddTo(_disposable);

            bullet.MoveProjectile();
        }

        private void CreateLaser()
        {
            if (IsReadyToShootLaser.Value)
            {
                var laser = _lasersPool.Get();

                laser.Init(_levelCanvas);
                
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

        public void Dispose()
        {
            _disposable.Dispose();
        }
    }
}