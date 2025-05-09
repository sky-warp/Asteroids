using System.Collections;
using _Project.Scripts.CustomPool;
using _Project.Scripts.Environment.Units;
using _Project.Scripts.Factories;
using _Project.Scripts.GameStateServices;
using _Project.Scripts.Infrastructure;
using _Project.Scripts.LevelBorder;
using _Project.Scripts.ParticleSystems;
using R3;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Project.Scripts.SpawnService
{
    public class EnvironmentUnitSpawnService : Zenject.IInitializable
    {
        public readonly ReactiveProperty<int> BigAsteroidScore = new();
        public readonly ReactiveProperty<int> SmallAsteroidScore = new();
        public readonly ReactiveProperty<int> UfoScore = new();

        private Transform _ufoTarget;

        private EnvironmentUnitPool<AsteroidBig> _bigAsteroidsPool;
        private EnvironmentUnitPool<AsteroidSmall> _smallAsteroidsPool;
        private EnvironmentUnitPool<UfoChaser> _ufoChasersPool;

        private DefaultGameStateService _defaultGameStateService;

        private SpawnRandomizer _spawnRandomizer;

        private DefaultVisualEffectSystem _defaultVisualEffectSystem;

        private LevelColliderBorder _levelColliderBorder;

        private CompositeDisposable _disposable = new();

        public EnvironmentUnitSpawnService(Transform ufoTarget,
            LevelColliderBorder levelColliderBorder,
            DefaultGameStateService defaultGameStateService,
            SpawnRandomizer spawnRandomizer,
            BaseMonoFactory<AsteroidBig> asteroidBigFactory,
            BaseMonoFactory<AsteroidSmall> asteroidSmallFactory,
            BaseMonoFactory<UfoChaser> ufoChaserFactory,
            DefaultVisualEffectSystem visualEffectSystem
        )
        {
            _spawnRandomizer = spawnRandomizer;

            _defaultGameStateService = defaultGameStateService;

            _ufoTarget = ufoTarget;

            _levelColliderBorder = levelColliderBorder;

            _defaultVisualEffectSystem = visualEffectSystem;
            _defaultVisualEffectSystem.CreateUnitEffects(levelColliderBorder.transform);

            _bigAsteroidsPool =
                new EnvironmentUnitPool<AsteroidBig>(3, _spawnRandomizer.GetRandomSpawnTransform(), asteroidBigFactory);
            _smallAsteroidsPool =
                new EnvironmentUnitPool<AsteroidSmall>(3, _spawnRandomizer.GetRandomSpawnTransform(),
                    asteroidSmallFactory);
            _ufoChasersPool =
                new EnvironmentUnitPool<UfoChaser>(3, _spawnRandomizer.GetRandomSpawnTransform(), ufoChaserFactory);
        }

        public void Initialize()
        {
            _levelColliderBorder.OnBigAsteroidExit
                .Subscribe(DeleteBigAsteroid)
                .AddTo(_disposable);
            _levelColliderBorder.OnSmallAsteroidExit
                .Subscribe(DeleteSmallAsteroid)
                .AddTo(_disposable);
        }

        private void GameOver()
        {
            _defaultGameStateService.OnGameOver?
                .OnNext(Unit.Default);

            _bigAsteroidsPool.ReleaseAll();
            _smallAsteroidsPool.ReleaseAll();
            _ufoChasersPool.ReleaseAll();
        }

        private void CreateBigAsteroid()
        {
            var asteroid = _bigAsteroidsPool.Get();

            var spawnPoint = _spawnRandomizer.GetRandomSpawnTransform();

            asteroid.transform.position = spawnPoint.position;

            float yDirection = (spawnPoint.position.y > 0) ? -1 : 1;

            float xDirection = Random.Range(-1f, 1f);

            if (spawnPoint.position.x > 0)
            {
                xDirection = -Mathf.Abs(xDirection);
            }
            else if (spawnPoint.position.x < 0)
            {
                xDirection = Mathf.Abs(xDirection);
            }

            Vector2 asteroidDirection = new Vector2(xDirection, yDirection).normalized;

            asteroid.ResetSubscription();

            var gameOverSubscription = asteroid.OnSpaceshipTouched
                .Subscribe(_ => GameOver());

            var hitSubscription = asteroid.OnBigAsteroidHit
                .Subscribe(_ =>
                    CreateSmallAsteroids(asteroid, asteroid.transform.position, asteroid.SmallAsteroidsAmountAfterHit));

            var playEffectSubscription = asteroid.UnitPositionWhenHit
                .Subscribe(_defaultVisualEffectSystem.PlayUnitDestroyEffect)
                .AddTo(_disposable);

            var stopBigAsterSubscription = _defaultGameStateService.OnGamePaused
                .Subscribe(_ => asteroid.StopObject())
                .AddTo(_disposable);
            
            var moveBigAsterSubscription = _defaultGameStateService.OnGameResume
                .Subscribe(_ => asteroid.MoveAsteroidBig(asteroidDirection))
                .AddTo(_disposable);

            asteroid.AddSubscription(hitSubscription);
            asteroid.AddSubscription(gameOverSubscription);
            asteroid.AddSubscription(playEffectSubscription);
            asteroid.AddSubscription(stopBigAsterSubscription);
            asteroid.AddSubscription(moveBigAsterSubscription);

            asteroid.MoveAsteroidBig(asteroidDirection);
        }

        private void DeleteBigAsteroid(AsteroidBig asteroidBig)
        {
            asteroidBig.ResetSubscription();

            _bigAsteroidsPool.Release(asteroidBig);
        }

        private void CreateSmallAsteroids(AsteroidBig shootedAsteroid, Vector3 startPosition, int amount)
        {
            BigAsteroidScore.Value = 0;
            BigAsteroidScore.Value = shootedAsteroid.Score;

            for (int i = 0; i < amount; i++)
            {
                var asteroidSmall = _smallAsteroidsPool.Get();

                asteroidSmall.ResetSubscription();

                var hitSubscription = asteroidSmall.OnSmallAsteroidHit
                    .Subscribe(TakeScoreForSmallAsteroidHit)
                    .AddTo(_disposable);

                var gameOverSubscription = asteroidSmall.OnSpaceshipTouched
                    .Subscribe(_ => GameOver());

                var playEffectSubscription = asteroidSmall.UnitPositionWhenHit
                    .Subscribe(_defaultVisualEffectSystem.PlayUnitDestroyEffect)
                    .AddTo(_disposable);
                
                var stopSmallAsteroidSubscription = _defaultGameStateService.OnGamePaused
                    .Subscribe(_ => asteroidSmall.StopObject())
                    .AddTo(_disposable);
                
                var moveSmallAsteroidSubscription = _defaultGameStateService.OnGameResume
                    .Subscribe(_ => asteroidSmall.MoveSmallAsteroid(startPosition))
                    .AddTo(_disposable);

                asteroidSmall.AddSubscription(hitSubscription);
                asteroidSmall.AddSubscription(gameOverSubscription);
                asteroidSmall.AddSubscription(playEffectSubscription);
                asteroidSmall.AddSubscription(stopSmallAsteroidSubscription);
                asteroidSmall.AddSubscription(moveSmallAsteroidSubscription);

                asteroidSmall.MoveSmallAsteroid(startPosition);
            }

            DeleteBigAsteroid(shootedAsteroid);
        }

        private void DeleteSmallAsteroid(AsteroidSmall asteroidSmall)
        {
            asteroidSmall.ResetSubscription();

            _smallAsteroidsPool.Release(asteroidSmall);
        }

        private void TakeScoreForSmallAsteroidHit(AsteroidSmall asteroidSmall)
        {
            DeleteSmallAsteroid(asteroidSmall);

            SmallAsteroidScore.Value = 0;
            SmallAsteroidScore.Value = asteroidSmall.Score;
        }

        private void CreateUfoChaser()
        {
            var ufoChaser = _ufoChasersPool.Get();

            ufoChaser.ResetSubscription();

            ufoChaser.MoveTowardsTarget();

            Observable
                .EveryUpdate()
                .Subscribe(_ => ufoChaser.TargetPosition.Value = _ufoTarget.position)
                .AddTo(ufoChaser.Disposable);

            ufoChaser.OnUfoHit
                .Subscribe(DeleteUfoChaser)
                .AddTo(ufoChaser.Disposable);

            ufoChaser.OnSpaceshipTouched
                .Subscribe(_ => GameOver())
                .AddTo(ufoChaser.Disposable);

            ufoChaser.UnitPositionWhenHit
                .Subscribe(_defaultVisualEffectSystem.PlayUnitDestroyEffect)
                .AddTo(ufoChaser.Disposable);

            _defaultGameStateService.OnGamePaused
                .Subscribe(_ => ufoChaser.StopChasing())
                .AddTo(ufoChaser.Disposable);
            
            _defaultGameStateService.OnGameResume
                .Subscribe(_ => ufoChaser.MoveTowardsTarget())
                .AddTo(ufoChaser.Disposable);
            
            var spawnPoint = _spawnRandomizer.GetRandomSpawnTransform();

            ufoChaser.transform.position = spawnPoint.position;
        }

        private void DeleteUfoChaser(UfoChaser ufoChaser)
        {
            ufoChaser.ResetSubscription();

            _ufoChasersPool.Release(ufoChaser);

            UfoScore.Value = 0;
            UfoScore.Value = ufoChaser.Score;
        }

        public IEnumerator SpawnBigAsteroids()
        {
            while (true)
            {
                float interval = Random.Range(0.5f, 1.5f);

                CreateBigAsteroid();

                yield return new WaitForSeconds(interval);
            }
        }

        public IEnumerator SpawnUfoChasers()
        {
            while (true)
            {
                int interval = Random.Range(3, 6);

                CreateUfoChaser();

                yield return new WaitForSeconds(interval);
            }
        }

        public void Dispose()
        {
            _disposable?.Dispose();
        }
    }
}