using UnityEngine;

namespace _Project.Scripts.Factories
{
    public class MonoFactory<T> where T : MonoBehaviour
    {
        private T _monoPrefab;

        public MonoFactory(T monoPrefab)
        {
            _monoPrefab = monoPrefab;
        }
        
        public T Create(Transform parent)
        {
            var instance = GameObject.Instantiate(_monoPrefab, parent);
            return instance;
        }
    }
}