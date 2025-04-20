using System.Collections.Generic;
using _Project.Scripts.Environment.Units;
using _Project.Scripts.Projectiles.ProjectileTypes;
using _Project.Scripts.Spaceship.View;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace _Project.Scripts.LocalAssetLoaders
{
    public class AssetLoader : IAssetLoadable
    {
        private const string BULLET_KEY = "Bullet";
        private const string LASER_KEY = "Laser";
        private const string ASTEROID_BIG_KEY = "AsteroidBig";
        private const string ASTEROID_SMALL_KEY = "AsteroidSmall";
        private const string UFO_CHASER_KEY = "UfoChaser";
        private const string SPACESHIP_KEY = "Spaceship";

        private List<AsyncOperationHandle> _loadedAssets = new();

        private GameObject _cachedGameObject;

        public async UniTask<Bullet> LoadBullet()
        {
            var op = Addressables.LoadAssetAsync<GameObject>(BULLET_KEY);

            _cachedGameObject = await op.Task;

            if (op.Status != AsyncOperationStatus.Succeeded &&
                _cachedGameObject.gameObject.TryGetComponent(out Bullet asset))
            {
                Debug.LogError($"Failed to load asset with key: {BULLET_KEY}");
            }

            _loadedAssets.Add(op);

            return _cachedGameObject.GetComponent<Bullet>();
        }

        public async UniTask<Laser> LoadLaser()
        {
            var op = Addressables.LoadAssetAsync<GameObject>(LASER_KEY);

            _cachedGameObject = await op.Task;

            if (op.Status != AsyncOperationStatus.Succeeded &&
                _cachedGameObject.gameObject.TryGetComponent(out Laser asset))
            {
                Debug.LogError($"Failed to load asset with key: {LASER_KEY}");
            }

            _loadedAssets.Add(op);

            return _cachedGameObject.GetComponent<Laser>();
        }

        public async UniTask<AsteroidBig> LoadAsteroidBig()
        {
            var op = Addressables.LoadAssetAsync<GameObject>(ASTEROID_BIG_KEY);

            _cachedGameObject = await op.Task;

            if (op.Status != AsyncOperationStatus.Succeeded &&
                _cachedGameObject.gameObject.TryGetComponent(out AsteroidBig asset))
            {
                Debug.LogError($"Failed to load asset with key: {ASTEROID_BIG_KEY}");
            }

            _loadedAssets.Add(op);

            return _cachedGameObject.GetComponent<AsteroidBig>();
        }

        public async UniTask<AsteroidSmall> LoadAsteroidSmall()
        {
            var op = Addressables.LoadAssetAsync<GameObject>(ASTEROID_SMALL_KEY);

            _cachedGameObject = await op.Task;

            if (op.Status != AsyncOperationStatus.Succeeded &&
                _cachedGameObject.gameObject.TryGetComponent(out AsteroidSmall asset))
            {
                Debug.LogError($"Failed to load asset with key: {ASTEROID_SMALL_KEY}");
            }

            _loadedAssets.Add(op);

            return _cachedGameObject.GetComponent<AsteroidSmall>();
        }

        public async UniTask<UfoChaser> LoadUfoChaser()
        {
            var op = Addressables.LoadAssetAsync<GameObject>(UFO_CHASER_KEY);

            _cachedGameObject = await op.Task;

            if (op.Status != AsyncOperationStatus.Succeeded &&
                _cachedGameObject.gameObject.TryGetComponent(out UfoChaser asset))
            {
                Debug.LogError($"Failed to load asset with key: {UFO_CHASER_KEY}");
            }

            _loadedAssets.Add(op);

            return _cachedGameObject.GetComponent<UfoChaser>();
        }

        public async UniTask<SpaceshipView> LoadSpaceship()
        {
            var op = Addressables.LoadAssetAsync<GameObject>(SPACESHIP_KEY);

            _cachedGameObject = await op.Task;

            if (op.Status != AsyncOperationStatus.Succeeded &&
                _cachedGameObject.gameObject.TryGetComponent(out SpaceshipView asset))
            {
                Debug.LogError($"Failed to load asset with key: {SPACESHIP_KEY}");
            }

            _loadedAssets.Add(op);

            return _cachedGameObject.GetComponent<SpaceshipView>();
        }

        public void UnloadAsset()
        {
            for (int i = 0; i < _loadedAssets.Count; i++)
            {
                Addressables.Release(_loadedAssets[i]);
            }
        }
    }
}