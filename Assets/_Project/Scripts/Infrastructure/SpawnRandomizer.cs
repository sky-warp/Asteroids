using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Project.Scripts.Infrastructure
{
    public class SpawnRandomizer
    {
        private Camera _camera;

        private float _minSpawnSpace;
        private float _maxSpawnSpace;
        
        public SpawnRandomizer(Camera camera, float minSpawnSpace, float maxSpawnSpace)
        {
            _camera = camera;
            
            _minSpawnSpace = minSpawnSpace;
            _maxSpawnSpace = maxSpawnSpace;
        }
        
        public Transform GetRandomSpawnTransform()
        {
            Vector2 bottomLeft = _camera.ViewportToWorldPoint(new Vector3(0, 0, _camera.nearClipPlane));
            Vector2 topRight = _camera.ViewportToWorldPoint(new Vector3(1, 1, _camera.nearClipPlane));
            
            Vector3 spawnPosition = Vector3.zero;
            
            SidesToSpawn randomSideToSpawn = (SidesToSpawn)Random.Range(0, Enum.GetValues(typeof(SidesToSpawn)).Length);

            switch (randomSideToSpawn)
            {
                case SidesToSpawn.Left: 
                    spawnPosition = new Vector3(
                        bottomLeft.x - Random.Range(_minSpawnSpace, _maxSpawnSpace),
                        Random.Range(bottomLeft.y, topRight.y),
                        0
                    );
                    break;
                case SidesToSpawn.Right: 
                    spawnPosition = new Vector3(
                        topRight.x + Random.Range(_minSpawnSpace, _maxSpawnSpace),
                        Random.Range(bottomLeft.y, topRight.y),
                        0
                    );
                    break;
                case SidesToSpawn.Top: 
                    spawnPosition = new Vector3(
                        Random.Range(bottomLeft.x, topRight.x),
                        topRight.y + Random.Range(_minSpawnSpace, _maxSpawnSpace),
                        0
                    );
                    break;
                case SidesToSpawn.Bottom: 
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
    
    public enum SidesToSpawn
    {
        Left,
        Right,
        Top,
        Bottom
    }
}