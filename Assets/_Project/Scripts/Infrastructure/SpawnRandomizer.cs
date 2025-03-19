using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace _Project.Scripts.Infrastructure
{
    public class SpawnRandomizer
    {
        [Inject] private Camera _camera;

        private float _minSpawnSpace = 1.0f;
        private float _maxSpawnSpace = 2.0f;

        public Transform GetRandomSpawnTransform()
        {
            Vector2 bottomLeft = _camera.ViewportToWorldPoint(new Vector3(0, 0, _camera.nearClipPlane));
            Vector2 topRight = _camera.ViewportToWorldPoint(new Vector3(1, 1, _camera.nearClipPlane));

            int side = Random.Range(0, 4);

            Vector3 spawnPosition = Vector3.zero;

            switch (side)
            {
                case 0: 
                    spawnPosition = new Vector3(
                        bottomLeft.x - Random.Range(_minSpawnSpace, _maxSpawnSpace),
                        Random.Range(bottomLeft.y, topRight.y),
                        0
                    );
                    break;
                case 1: 
                    spawnPosition = new Vector3(
                        topRight.x + Random.Range(_minSpawnSpace, _maxSpawnSpace),
                        Random.Range(bottomLeft.y, topRight.y),
                        0
                    );
                    break;
                case 2: 
                    spawnPosition = new Vector3(
                        Random.Range(bottomLeft.x, topRight.x),
                        topRight.y + Random.Range(_minSpawnSpace, _maxSpawnSpace),
                        0
                    );
                    break;
                case 3: 
                    spawnPosition = new Vector3(
                        Random.Range(bottomLeft.x, topRight.x),
                        bottomLeft.y - Random.Range(_minSpawnSpace, _maxSpawnSpace),
                        0
                    );
                    break;
            }

            GameObject dummySpawnPoint = new GameObject("SpawnPoint");
            dummySpawnPoint.transform.position = spawnPosition;
            
            dummySpawnPoint.AddComponent<DummyForSpawnTimer>();
            
            return dummySpawnPoint.transform;
        }
    }
}