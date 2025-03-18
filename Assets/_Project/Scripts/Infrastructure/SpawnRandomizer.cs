using UnityEngine;
using Zenject;

namespace _Project.Scripts.Infrastructure
{
    public class SpawnRandomizer
    {
        [Inject] Camera _camera;

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
                        bottomLeft.x - Random.Range(1f, 3f),
                        Random.Range(bottomLeft.y, topRight.y),
                        0
                    );
                    break;
                case 1: 
                    spawnPosition = new Vector3(
                        topRight.x + Random.Range(1f, 3f),
                        Random.Range(bottomLeft.y, topRight.y),
                        0
                    );
                    break;
                case 2: 
                    spawnPosition = new Vector3(
                        Random.Range(bottomLeft.x, topRight.x),
                        topRight.y + Random.Range(1f, 3f),
                        0
                    );
                    break;
                case 3: 
                    spawnPosition = new Vector3(
                        Random.Range(bottomLeft.x, topRight.x),
                        bottomLeft.y - Random.Range(1f, 3f),
                        0
                    );
                    break;
            }

            GameObject spawnPoint = new GameObject("SpawnPoint");
            spawnPoint.transform.position = spawnPosition;

            return spawnPoint.transform;
        }
    }
}