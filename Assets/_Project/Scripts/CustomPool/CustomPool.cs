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
        private List<T> _pool;
        private int _count;

        public CustomPool(T prefab, int prewarnObjects, Transform parent)
        {
            _prefab = prefab;
            _parent = parent;
            _pool = new List<T>();

            _count = prewarnObjects;

            for (int i = 0; i < prewarnObjects; i++)
            {
                var obj = GameObject.Instantiate(_prefab, _parent);
                obj.gameObject.SetActive(false);
                _pool.Add(obj);
            }
        }

        public T Get()
        {
            var obj = _pool.FirstOrDefault(x => !x.isActiveAndEnabled);


            if (obj == null)
                obj = Create();

            obj.gameObject.SetActive(true);

            return obj;
        }

        public void Release(T obj)
        {
            _pool.Remove(obj);

            /*if (_count > 3)
            {
                while (_pool.Count > 3)
                {
                    var currentItem = _pool[_pool.Count - 1];
                    _pool.RemoveAt(_pool.Count - 1);
                    GameObject.Destroy(currentItem.gameObject);
                }
            }*/
        }

        public T Create()
        {
            var obj = GameObject.Instantiate(_prefab, _parent);
            _pool.Add(obj);
            _count++;
            return obj;
        }
    }
}