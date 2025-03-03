using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace _Project.Scripts.CustomPool
{
    public class CustomPool<T> where T : MonoBehaviour
    {
        private T _prefab;
        private Transform _parent;
        private List<T> Pool { get; }

        public CustomPool(T prefab, int prewarnObjects, Transform parent)
        {
            _prefab = prefab;
            _parent = parent;
            Pool = new List<T>();


            for (int i = 0; i < prewarnObjects; i++)
            {
                var obj = GameObject.Instantiate(_prefab, _parent);
                obj.gameObject.SetActive(false);
                Pool.Add(obj);
            }
        }

        public T Get()
        {
            var obj = Pool.FirstOrDefault(obj => !obj.gameObject.activeSelf);


            if (obj == null)
                obj = Create();

            obj.gameObject.SetActive(true);

            return obj;
        }

        public void Release(T obj)
        {
            obj.transform.localPosition = Vector3.zero;
            obj.transform.localRotation = Quaternion.identity;
            obj.gameObject.SetActive(false);
        }

        public void ReleaseAll()
        {
            foreach (var t in Pool)
            {
                Release(t);
            }
        }
        
        private T Create()
        {
            var obj = GameObject.Instantiate(_prefab, _parent);
            Pool.Add(obj);
            return obj;
        }
    }
}