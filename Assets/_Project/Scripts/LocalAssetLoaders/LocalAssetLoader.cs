using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace _Project.Scripts.LocalAssetLoaders
{
    public class LocalAssetLoader
    {
        private GameObject _cachedGameObject;
        
        public async Task<GameObject> LoadAssets(string key)
        {
            var op = Addressables.LoadAssetAsync<GameObject>(key);
            
            _cachedGameObject = await op.Task; 
            
            if (op.Status != AsyncOperationStatus.Succeeded)
            {
                Debug.LogError($"Failed to load asset with key: {key}");
                return null;
            }
            
            return _cachedGameObject;
        }
    }

    public class ResourcesLoader
    {
        public GameObject Laser { get; set; }
        public LocalAssetLoader LocalAssetLoader { get; private set; }
        
        
        public ResourcesLoader(LocalAssetLoader loader)
        {
            LocalAssetLoader = loader;
        }
    }
} 