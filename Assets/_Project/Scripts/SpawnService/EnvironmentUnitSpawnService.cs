using System.Collections;
using _Project.Scripts.CustomPool;
using _Project.Scripts.Environment.EnvironmentUnitTypes;
using _Project.Scripts.LevelBorder;
using R3;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Project.Scripts.SpawnService
{
    public class EnvironmentUnitSpawnService : MonoBehaviour
    {
        public readonly Subject<int> OnScoreChanged = new();

        [Header("Pause service")] [SerializeField]
        private PauseGameService.PauseGameService _pauseGameService;

        [Header("Types of environment unit")] [SerializeField]
        private AsteroidBig _asteroidBigPrefab;

        [SerializeField] private AsteroidSmall _asteroidSmallPrefab;
        [SerializeField] private UfoChaser _ufoChaserPrefab;

        [Header("Level area")] [SerializeField]
        private Transform _environmentParent;

        [Header("Spawn points")] [SerializeField]
        private Transform[] _spawnPoints;

        [Header("Level's border")] [SerializeField]
        private LevelColliderBorder _levelColliderBorder;

        [Header("Ufo target")] [SerializeField]
        private Transform _ufoTarget;

        private CustomPool<AsteroidBig> _bigAsteroidsPool;
        private CustomPool<AsteroidSmall> _smallAsteroidsPool;
        private CustomPool<UfoChaser> _ufoChasersPool;

        private void Awake()
        {
            _bigAsteroidsPool = new CustomPool<AsteroidBig>(_asteroidBigPrefab, 3, _environmentParent);
            _smallAsteroidsPool = new CustomPool<AsteroidSmall>(_asteroidSmallPrefab, 3, _environmentParent);

            StartCoroutine(SpawnBigAsteroids());

            _levelColliderBorder.OnBigAsteroidExit
                .Subscribe(DeleteBigAsteroid)
                .AddTo(this);
            _levelColliderBorder.OnSmallAsteroidExit
                .Subscribe(DeleteSmallAsteroid)
                .AddTo(this);

            _ufoChasersPool = new CustomPool<UfoChaser>(_ufoChaserPrefab, 3, _environmentParent);

            StartCoroutine(SpawnUfoChasers());
        }

        private void GameOver()
        {
            _bigAsteroidsPool.ReleaseAll();
            _smallAsteroidsPool.ReleaseAll();
            _ufoChasersPool.ReleaseAll();

            _pauseGameService.OnPause
                .OnNext(Unit.Default);

            StopAllCoroutines();
        }

        private void CreateBigAsteroid()
        {
            var asteroid = _bigAsteroidsPool.Get();

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
            OnScoreChanged?.OnNext(shootedAsteroid.Score);

            for (int i = 0; i < amount; i++)
            {
                var asteroidSmall = _smallAsteroidsPool.Get();

                asteroidSmall.ResetSubscription();

                var hitSubscription = asteroidSmall.OnSmallAsteroidHit
                    .Subscribe(TakeScoreForSmallAsteroidHit)
                    .AddTo(this);

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

            OnScoreChanged?.OnNext(asteroidSmall.Score);
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

            var spawnPoint = _spawnPoints[Random.Range(0, _spawnPoints.Length)];

            ufoChaser.transform.SetParent(spawnPoint.transform);
            ufoChaser.transform.position = spawnPoint.position;
        }

        private void DeleteUfoChaser(UfoChaser ufoChaser)
        {
            ufoChaser.ResetSubscription();

            _ufoChasersPool.Release(ufoChaser);

            OnScoreChanged?.OnNext(ufoChaser.Score);
        }

        private IEnumerator SpawnBigAsteroids()
        {
            while (true)
            {
                float interval = Random.Range(0.5f, 1.5f);

                CreateBigAsteroid();

                yield return new WaitForSeconds(interval);
            }
        }

        private IEnumerator SpawnUfoChasers()
        {
            while (true)
            {
                int interval = Random.Range(3, 6);

                CreateUfoChaser();

                yield return new WaitForSeconds(interval);
            }
        }
    }
}