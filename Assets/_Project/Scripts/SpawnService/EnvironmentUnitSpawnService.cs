using System.Collections;
using _Project.Scripts.CustomPool;
using _Project.Scripts.Environment.EnvironmentUnitTypes;
using _Project.Scripts.LevelBorder;
using R3;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Project.Scripts.SpawnService
{
    public class EnvironmentUnitSpawnService
    {
        public readonly Subject<int> OnScoreChanged = new();
        
        public readonly ReactiveProperty<int> BigAsteroidScore = new();
        public readonly ReactiveProperty<int> SmallAsteroidScore = new();
        public readonly ReactiveProperty<int> UfoScore = new();

        private Transform[] _spawnPoints;

        private Transform _ufoTarget;

        private CustomPool<AsteroidBig> _bigAsteroidsPool;
        private CustomPool<AsteroidSmall> _smallAsteroidsPool;
        private CustomPool<UfoChaser> _ufoChasersPool;

        private CompositeDisposable _disposable = new();

        private PauseGameService.PauseGame _pauseGame;
        
        private Canvas _levelCanvas;

        public EnvironmentUnitSpawnService(AsteroidBig asteroidBigPrefab, AsteroidSmall asteroidSmallPrefab,
            UfoChaser ufoChaserPrefab, Transform environmentParent, Transform ufoTarget,
            LevelColliderBorder levelColliderBorder, Transform[] spawnPoints,
            PauseGameService.PauseGame pauseGame, Canvas levelCanvas)
        {
            _levelCanvas = levelCanvas;
            
            _pauseGame = pauseGame;
            
            _spawnPoints = spawnPoints;
            _ufoTarget = ufoTarget;

            _bigAsteroidsPool = new CustomPool<AsteroidBig>(asteroidBigPrefab, 3, environmentParent);
            _smallAsteroidsPool = new CustomPool<AsteroidSmall>(asteroidSmallPrefab, 3, environmentParent);

            levelColliderBorder.OnBigAsteroidExit
                .Subscribe(DeleteBigAsteroid)
                .AddTo(_disposable);
            levelColliderBorder.OnSmallAsteroidExit
                .Subscribe(DeleteSmallAsteroid)
                .AddTo(_disposable);

            _ufoChasersPool = new CustomPool<UfoChaser>(ufoChaserPrefab, 3, environmentParent);
        }

        private void GameOver()
        {
            _pauseGame.OnPause?
                .OnNext(Unit.Default);

            _bigAsteroidsPool.ReleaseAll();
            _smallAsteroidsPool.ReleaseAll();
            _ufoChasersPool.ReleaseAll();
        }

        private void CreateBigAsteroid()
        {
            var asteroid = _bigAsteroidsPool.Get();

            asteroid.Init(_levelCanvas);
            
            asteroid.ResetSubscription();

            var gameOverSubscription = asteroid.OnSpaceshipTouched
                .Subscribe(_ => GameOver());

            var hitSubscription = asteroid.OnBigAsteroidHit
                .Subscribe(_ =>
                    CreateSmallAsteroids(asteroid, asteroid.transform.position, asteroid.SmallAsteroidsAmountAfterHit));

            asteroid.AddSubscription(hitSubscription);
            asteroid.AddSubscription(gameOverSubscription);

            var spawnPoint = _spawnPoints[Random.Range(0, _spawnPoints.Length)];

            asteroid.transform.SetParent(spawnPoint.transform);
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

                asteroidSmall.Init(_levelCanvas);
                
                asteroidSmall.ResetSubscription();

                var hitSubscription = asteroidSmall.OnSmallAsteroidHit
                    .Subscribe(TakeScoreForSmallAsteroidHit)
                    .AddTo(_disposable);

                var gameOverSubscription = asteroidSmall.OnSpaceshipTouched
                    .Subscribe(_ => GameOver());

                asteroidSmall.AddSubscription(hitSubscription);
                asteroidSmall.AddSubscription(gameOverSubscription);

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

            ufoChaser.Init(_levelCanvas);
            
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

            var spawnPoint = _spawnPoints[Random.Range(0, _spawnPoints.Length)];

            ufoChaser.transform.SetParent(spawnPoint.transform);
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