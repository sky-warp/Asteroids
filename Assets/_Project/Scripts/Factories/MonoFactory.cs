using UnityEngine;

namespace _Project.Scripts.Factories
{
    public class MonoFactory<T> where T : MonoBehaviour
    {
        private T _prefab;

        public MonoFactory(T prefab)
        {
            _prefab = prefab;
        }
        
        public T Create(Transform parent)
        {
            var instance = GameObject.Instantiate(_prefab, parent);
            return instance;
        }
    }
}