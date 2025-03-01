using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace _Project.Scripts.CustomPool
{
    public class CustomPool<T> where T : MonoBehaviour
    {
        //1. INIT - creating a pool. Which objects pool will contain, do we need to pre-instantiate certain amount of them
        //2. GET - find a non-use object and change it status to "in-use" (when player shoots we change bullet status)
        //3. RELEASE - deactivating object, it's status return to "non-use" and object itself will be deleted from scene when bullet hits the enemy or gets out of screen bounds  
        //4/ CREATE - create object in pull if there's no free object (often works with GET command, so we are trying to get free object - if we cant, then we need to create one

        private T _prefab;
        private Transform _parent;
        public List<T> Pool { get; }

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
            //Pool.Remove(obj);
        }

        public T Create()
        {
            var obj = GameObject.Instantiate(_prefab, _parent);
            Pool.Add(obj);
            return obj;
        }
    }
}