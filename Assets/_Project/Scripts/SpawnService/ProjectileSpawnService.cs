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
    public class ProjectileSpawnService : Zenject.IInitializable
    {
        public readonly Subject<Unit> OnLaserSpawned = new();
        public readonly Subject<Unit> OnBulletSpawned = new();
        public readonly ReactiveProperty<bool> IsReadyToShootLaser = new();

        private ProjectilePool<Bullet> _bulletsPool;
        private ProjectilePool<Laser> _lasersPool;

        private IInputable _inputManager;
        
        private DefaultAudioManager _audioManager;
        
        private DefaultVisualEffectSystem _visualEffectSystem;

        private DefaultGameStateService _gameStateService;
        
        private LevelColliderBorder _levelColliderBorder;
        
        private readonly CompositeDisposable _disposable = new();

        public ProjectileSpawnService(IInputable inputManager,
            LevelColliderBorder levelBorder, 
            Transform shipTransform,
            DefaultGameStateService defaultGameStateService, 
            BaseMonoFactory<Bullet> bulletFactory,
            BaseMonoFactory<Laser> laserFactory,
            DefaultAudioManager audioManager,
            DefaultVisualEffectSystem visualEffectSystem)
        {
            _inputManager = inputManager;
            _audioManager = audioManager;
            _visualEffectSystem = visualEffectSystem;
            _gameStateService = defaultGameStateService;
            _levelColliderBorder = levelBorder;
            
            _visualEffectSystem.CreateProjectileVisualEffects(shipTransform);

            _bulletsPool = new ProjectilePool<Bullet>(3, shipTransform, bulletFactory);
            _lasersPool = new ProjectilePool<Laser>(3, shipTransform, laserFactory);
        }

        public void Initialize()
        {
            _gameStateService.OnGameOver
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
            
            _levelColliderBorder.OnBulletExit
                .Subscribe(DeleteSpawnedBullet)
                .AddTo(_disposable);
            _levelColliderBorder.OnLaserExit
                .Subscribe(DeleteSpawnedLaser)
                .AddTo(_disposable);
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