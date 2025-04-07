using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace _Project.Scripts.LocalAssetLoaders
{
    public class LocalAssetLoader : ILocalAssetLoadable
    {
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

            return _cachedGameObject.GetComponent<T>();
        }
    }
}