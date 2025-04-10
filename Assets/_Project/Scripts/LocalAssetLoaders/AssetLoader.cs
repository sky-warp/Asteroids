using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace _Project.Scripts.LocalAssetLoaders
{
    public class AssetLoader : IAssetLoadable
    {
        private List<AsyncOperationHandle> _loadedAssets = new();

        private GameObject _cachedGameObject;

        public async UniTask<Bullet> LoadBullet<Bullet>(string key)
        {
            var op = Addressables.LoadAssetAsync<GameObject>(key);

            _cachedGameObject = await op.Task;

            if (op.Status != AsyncOperationStatus.Succeeded &&
                _cachedGameObject.gameObject.TryGetComponent(out Bullet asset))
            {
                Debug.LogError($"Failed to load asset with key: {key}");
            }

            _loadedAssets.Add(op);

            return _cachedGameObject.GetComponent<Bullet>();
        }

        public async UniTask<Laser> LoadLaser<Laser>(string key)
        {
            var op = Addressables.LoadAssetAsync<GameObject>(key);

            _cachedGameObject = await op.Task;

            if (op.Status != AsyncOperationStatus.Succeeded &&
                _cachedGameObject.gameObject.TryGetComponent(out Laser asset))
            {
                Debug.LogError($"Failed to load asset with key: {key}");
            }

            _loadedAssets.Add(op);

            return _cachedGameObject.GetComponent<Laser>();
        }

        public async UniTask<AsteroidBig> LoadAsteroidBig<AsteroidBig>(string key)
        {
            var op = Addressables.LoadAssetAsync<GameObject>(key);

            _cachedGameObject = await op.Task;

            if (op.Status != AsyncOperationStatus.Succeeded &&
                _cachedGameObject.gameObject.TryGetComponent(out AsteroidBig asset))
            {
                Debug.LogError($"Failed to load asset with key: {key}");
            }

            _loadedAssets.Add(op);

            return _cachedGameObject.GetComponent<AsteroidBig>();
        }

        public async UniTask<AsteroidSmall> LoadAsteroidSmall<AsteroidSmall>(string key)
        {
            var op = Addressables.LoadAssetAsync<GameObject>(key);

            _cachedGameObject = await op.Task;

            if (op.Status != AsyncOperationStatus.Succeeded &&
                _cachedGameObject.gameObject.TryGetComponent(out AsteroidSmall asset))
            {
                Debug.LogError($"Failed to load asset with key: {key}");
            }

            _loadedAssets.Add(op);

            return _cachedGameObject.GetComponent<AsteroidSmall>();
        }

        public async UniTask<UfoChaser> LoadUfoChaser<UfoChaser>(string key)
        {
            var op = Addressables.LoadAssetAsync<GameObject>(key);

            _cachedGameObject = await op.Task;

            if (op.Status != AsyncOperationStatus.Succeeded &&
                _cachedGameObject.gameObject.TryGetComponent(out UfoChaser asset))
            {
                Debug.LogError($"Failed to load asset with key: {key}");
            }

            _loadedAssets.Add(op);

            return _cachedGameObject.GetComponent<UfoChaser>();
        }

        public async UniTask<SpaceshipView> LoadSpaceship<SpaceshipView>(string key)
        {
            var op = Addressables.LoadAssetAsync<GameObject>(key);

            _cachedGameObject = await op.Task;

            if (op.Status != AsyncOperationStatus.Succeeded &&
                _cachedGameObject.gameObject.TryGetComponent(out SpaceshipView asset))
            {
                Debug.LogError($"Failed to load asset with key: {key}");
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