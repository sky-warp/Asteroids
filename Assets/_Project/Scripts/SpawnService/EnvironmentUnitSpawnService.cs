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
        [SerializeField] private AsteroidBig _asteroidBigPrefab;
        [SerializeField] private AsteroidSmall _asteroidSmallPrefab;
        [SerializeField] private Transform _environmentParent; 
        [SerializeField] private Transform[] _spawnPoints; 
        
        [SerializeField] private LevelColliderBorder _levelColliderBorder;
        
        private CustomPool<AsteroidBig> _asteroidPool;
        private bool _isEnough;
        private CompositeDisposable _disposable = new();
        
        private void Start()
        {
            _asteroidPool = new CustomPool<AsteroidBig>(_asteroidBigPrefab, 3, _environmentParent);
            StartCoroutine(SpawnBigAsteroids());
            
            _levelColliderBorder.OnBigAsteroidExit
                .Subscribe(DeleteBigAsteroid)
                .AddTo(this);
        }

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.Escape))
                _isEnough = true;
        }

        private void CreateAsteroid()
        {
            var asteroid = _asteroidPool.Get();
            
            asteroid.OnBigAsteroidHit
                .Subscribe(_ => CreateSmallAsteroids(asteroid, asteroid.transform.position))
                .AddTo(_disposable);
            
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
            _asteroidPool.Release(asteroidBig);
        }

        private void CreateSmallAsteroids(AsteroidBig shootedAsteroid, Vector3 startPosition)
        {
            DeleteBigAsteroid(shootedAsteroid);
            
            AsteroidSmall[] smallAsteroids = new AsteroidSmall[3];

            for (int i = 0; i < smallAsteroids.Length; i++)
            {
                smallAsteroids[i] = Instantiate(_asteroidSmallPrefab, _environmentParent);
            }

            foreach (var asteroid in smallAsteroids)
            {
                asteroid.MoveSmallAsteroid(startPosition);
            }
        }
        
        public IEnumerator SpawnBigAsteroids()
        {
            while (!_isEnough)
            {
                float interval = Random.Range(0.5f, 1.5f);
                
                CreateAsteroid();
                
                yield return new WaitForSeconds(interval);
            }
        }

        private void OnDestroy()
        {
            _disposable?.Dispose();
            StopAllCoroutines();
        }
    }
}