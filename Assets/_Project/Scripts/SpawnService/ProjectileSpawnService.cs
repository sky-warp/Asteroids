using _Project.Scripts.AudioSystems;
using _Project.Scripts.CustomPool;
using _Project.Scripts.Factories;
using _Project.Scripts.GameOverServices;
using _Project.Scripts.InputService;
using _Project.Scripts.LevelBorder;
using _Project.Scripts.ParticleSystems;
using _Project.Scripts.Projectiles.ProjectileTypes;
using R3;
using UnityEngine;

namespace _Project.Scripts.SpawnService
{
    public class ProjectileSpawnService
    {
        public readonly Subject<Unit> OnLaserSpawned = new();
        public readonly Subject<Unit> OnBulletSpawned = new();
        public readonly ReactiveProperty<bool> IsReadyToShootLaser = new();

        private CustomPool<Bullet> _bulletsPool;
        private CustomPool<Laser> _lasersPool;
        private readonly CompositeDisposable _disposable = new();

        private IInputable _inputManager;
        
        private DefaultAudioManager _audioManager;
        
        private DefaultVisualEffectSystem _visualEffectSystem;

        public ProjectileSpawnService(IInputable inputManager,
            LevelColliderBorder levelBorder, 
            Transform shipTransform,
            DefaultGameStateService defaultGameStateService, 
            MonoFactory<Bullet> bulletFactory,
            MonoFactory<Laser> laserFactory,
            DefaultAudioManager audioManager,
            DefaultVisualEffectSystem visualEffectSystem)
        {
            _inputManager = inputManager;
            _audioManager = audioManager;
            _visualEffectSystem = visualEffectSystem;
            
            _visualEffectSystem.CreateVisualEffects(shipTransform);
            
            defaultGameStateService.OnGameOver
                .Subscribe(_ => GameOver())
                .AddTo(_disposable);

            _inputManager.OnBulletRelease += CreateBullet;
            _inputManager.OnLaserRelease += CreateLaser;

            OnLaserSpawned
                .Subscribe(_ => _audioManager.PlayLaserSound())
                .AddTo(_disposable);
            OnLaserSpawned
                .Subscribe(_ => _visualEffectSystem.PlayGunShootEffect())
                .AddTo(_disposable);
            
            OnBulletSpawned
                .Subscribe(_ => _audioManager.PlayBulletSound())
                .AddTo(_disposable);
            OnBulletSpawned
                .Subscribe(_ => _visualEffectSystem.PlayGunShootEffect())
                .AddTo(_disposable);
            
            levelBorder.OnBulletExit
                .Subscribe(DeleteSpawnedBullet)
                .AddTo(_disposable);
            levelBorder.OnLaserExit
                .Subscribe(DeleteSpawnedLaser)
                .AddTo(_disposable);

            _bulletsPool = new CustomPool<Bullet>(3, shipTransform, bulletFactory);
            _lasersPool = new CustomPool<Laser>(3, shipTransform, laserFactory);
        }

        private void GameOver()
        {
            _bulletsPool.ReleaseAll();
            _lasersPool.ReleaseAll();
        }

        private void CreateBullet()
        {
            var bullet = _bulletsPool.Get();

            bullet.OnUnitHit
                .Subscribe(unit => _visualEffectSystem.PlayUnitDestroyEffect(unit.transform))
                .AddTo(_disposable);
            bullet.OnUnitHit
                .Subscribe(projectile => DeleteSpawnedBullet((Bullet)projectile))
                .AddTo(_disposable);
            bullet.OnUnitHit
                .Subscribe(_ => _audioManager.PlayScoreEarnSound())
                .AddTo(_disposable);

            bullet.MoveProjectile();
            
            OnBulletSpawned?.OnNext(Unit.Default);
        }

        private void CreateLaser()
        {
            if (IsReadyToShootLaser.Value)
            {
                var laser = _lasersPool.Get();

                laser.OnUnitHit
                    .Subscribe(_ => _audioManager.PlayScoreEarnSound())
                    .AddTo(_disposable);
                laser.OnUnitHit
                    .Subscribe(unit => _visualEffectSystem.PlayUnitDestroyEffect(unit.transform))
                    .AddTo(_disposable);
                
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
            _inputManager.OnBulletRelease -= CreateBullet;
            _inputManager.OnLaserRelease -= CreateLaser;

            _disposable.Dispose();
        }
    }
}