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
        #region Assets key
        private const string BulletKey = "Bullet";
        private const string LaserKey = "Laser";
        private const string AsteroidBigKey = "AsteroidBig";
        private const string AsteroidSmallKey = "AsteroidSmall";
        private const string UfoChaserKey = "UfoChaser";
        private const string SpaceshipKey = "Spaceship";
        #endregion

        private List<AsyncOperationHandle> _loadedAssets = new();

        private GameObject _cachedGameObject;

        public async UniTask<Bullet> LoadBullet()
        {
            var op = Addressables.LoadAssetAsync<GameObject>(BulletKey);

            _cachedGameObject = await op.Task;

            if (op.Status != AsyncOperationStatus.Succeeded &&
                _cachedGameObject.gameObject.TryGetComponent(out Bullet asset))
            {
                Debug.LogError($"Failed to load asset with key: {BulletKey}");
            }

            _loadedAssets.Add(op);

            return _cachedGameObject.GetComponent<Bullet>();
        }

        public async UniTask<Laser> LoadLaser()
        {
            var op = Addressables.LoadAssetAsync<GameObject>(LaserKey);

            _cachedGameObject = await op.Task;

            if (op.Status != AsyncOperationStatus.Succeeded &&
                _cachedGameObject.gameObject.TryGetComponent(out Laser asset))
            {
                Debug.LogError($"Failed to load asset with key: {LaserKey}");
            }

            _loadedAssets.Add(op);

            return _cachedGameObject.GetComponent<Laser>();
        }

        public async UniTask<AsteroidBig> LoadAsteroidBig()
        {
            var op = Addressables.LoadAssetAsync<GameObject>(AsteroidBigKey);

            _cachedGameObject = await op.Task;

            if (op.Status != AsyncOperationStatus.Succeeded &&
                _cachedGameObject.gameObject.TryGetComponent(out AsteroidBig asset))
            {
                Debug.LogError($"Failed to load asset with key: {AsteroidBigKey}");
            }

            _loadedAssets.Add(op);

            return _cachedGameObject.GetComponent<AsteroidBig>();
        }

        public async UniTask<AsteroidSmall> LoadAsteroidSmall()
        {
            var op = Addressables.LoadAssetAsync<GameObject>(AsteroidSmallKey);

            _cachedGameObject = await op.Task;

            if (op.Status != AsyncOperationStatus.Succeeded &&
                _cachedGameObject.gameObject.TryGetComponent(out AsteroidSmall asset))
            {
                Debug.LogError($"Failed to load asset with key: {AsteroidSmallKey}");
            }

            _loadedAssets.Add(op);

            return _cachedGameObject.GetComponent<AsteroidSmall>();
        }

        public async UniTask<UfoChaser> LoadUfoChaser()
        {
            var op = Addressables.LoadAssetAsync<GameObject>(UfoChaserKey);

            _cachedGameObject = await op.Task;

            if (op.Status != AsyncOperationStatus.Succeeded &&
                _cachedGameObject.gameObject.TryGetComponent(out UfoChaser asset))
            {
                Debug.LogError($"Failed to load asset with key: {UfoChaserKey}");
            }

            _loadedAssets.Add(op);

            return _cachedGameObject.GetComponent<UfoChaser>();
        }

        public async UniTask<SpaceshipView> LoadSpaceship()
        {
            var op = Addressables.LoadAssetAsync<GameObject>(SpaceshipKey);

            _cachedGameObject = await op.Task;

            if (op.Status != AsyncOperationStatus.Succeeded &&
                _cachedGameObject.gameObject.TryGetComponent(out SpaceshipView asset))
            {
                Debug.LogError($"Failed to load asset with key: {SpaceshipKey}");
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