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
        private bool _isEnough;

        private void Start()
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

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
                _isEnough = true;
        }

        private void CreateAsteroid()
        {
            var asteroid = _bigAsteroidsPool.Get();

            asteroid.ResetSubscription();

            var subscription = asteroid.OnBigAsteroidHit
                .Subscribe(_ => CreateSmallAsteroids(asteroid, asteroid.transform.position, asteroid.SmallAsteroidsAmountAfterHit));

            asteroid.AddSubscription(subscription);

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
            
            OnScoreChanged?.OnNext(asteroidBig.Score);
        }

        private void CreateSmallAsteroids(AsteroidBig shootedAsteroid, Vector3 startPosition, int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                var asteroidSmall = _smallAsteroidsPool.Get();
                
                asteroidSmall.OnSmallAsteroidHit
                    .Subscribe(DeleteSmallAsteroid)
                    .AddTo(this);
                
                asteroidSmall.MoveSmallAsteroid(startPosition);
            }
            
            DeleteBigAsteroid(shootedAsteroid);
            
            /*AsteroidSmall[] smallAsteroids = new AsteroidSmall[3];

            for (int i = 0; i < smallAsteroids.Length; i++)
            {
                smallAsteroids[i] = Instantiate(_asteroidSmallPrefab, _environmentParent);
            }

            foreach (var asteroid in smallAsteroids)
            {
                asteroid.MoveSmallAsteroid(startPosition);
            }*/
        }

        private void DeleteSmallAsteroid(AsteroidSmall asteroidSmall)
        {
            _smallAsteroidsPool.Release(asteroidSmall);
            OnScoreChanged?.OnNext(asteroidSmall.Score);
        }
        
        private void CreateUfoChaser()
        {
            var ufoChaser = _ufoChasersPool.Get();

            ufoChaser.MoveTowardsTarget();

            Observable
                .EveryUpdate()
                .Subscribe(_ => ufoChaser.TargetPosition.Value = _ufoTarget.position)
                .AddTo(this);

            ufoChaser.OnProjectileHitUfo
                .Subscribe(DeleteUfoChaser)
                .AddTo(this);

            var spawnPoint = _spawnPoints[Random.Range(0, _spawnPoints.Length)];

            ufoChaser.transform.SetParent(spawnPoint.transform);
            ufoChaser.transform.position = spawnPoint.position;
        }

        private void DeleteUfoChaser(UfoChaser ufoChaser)
        {
            _ufoChasersPool.Release(ufoChaser);
            
            OnScoreChanged?.OnNext(ufoChaser.Score);
        }

        private IEnumerator SpawnBigAsteroids()
        {
            while (!_isEnough)
            {
                float interval = Random.Range(0.5f, 1.5f);

                CreateAsteroid();

                yield return new WaitForSeconds(interval);
            }
        }

        private IEnumerator SpawnUfoChasers()
        {
            while (!_isEnough)
            {
                int interval = Random.Range(3, 6);

                CreateUfoChaser();

                yield return new WaitForSeconds(interval);
            }
        }

        private void OnDestroy()
        {
            StopAllCoroutines();
        }
    }
}