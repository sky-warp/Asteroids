using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace _Project.Scripts.LocalAssetLoaders
{
    public class AssetLoader : IAssetLoadable
    {
        private List<AsyncOperationHandle> _loadedAssets = new();
        
        private GameObject _cachedGameObject;

        public async Task<T> LoadAsset<T>(string key)
        {
            var op = Addressables.LoadAssetAsync<GameObject>(key);

            _cachedGameObject = await op.Task;

            if (op.Status != AsyncOperationStatus.Succeeded &&
                _cachedGameObject.gameObject.TryGetComponent<T>(out T asset))
            {
                Debug.LogError($"Failed to load asset with key: {key}");
            }

            _loadedAssets.Add(op);
            
            return _cachedGameObject.GetComponent<T>();
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