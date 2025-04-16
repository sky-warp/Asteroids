using UnityEngine;

namespace _Project.Scripts.Factories
{
    public abstract class BaseMonoFactory<T> where T : MonoBehaviour
    {
        private T _monoPrefab;

        public BaseMonoFactory(T monoPrefab)
        {
            _monoPrefab = monoPrefab;
        }

        public abstract T Create(Transform parent);
    }
}