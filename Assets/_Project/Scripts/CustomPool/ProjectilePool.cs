using System.Collections.Generic;
using System.Linq;
using _Project.Scripts.Factories;
using _Project.Scripts.Projectiles.ProjectileTypes;
using UnityEngine;

namespace _Project.Scripts.CustomPool
{
    public class ProjectilePool<T> where T : Projectile
    {
        private Transform _parent;
        private ProjectileFactory<T> _factory;
        private List<T> Pool { get; }
        
        public ProjectilePool(int prewarmObjects, Transform parent, ProjectileFactory<T> factory)
        {
            _factory = factory;
            _parent = parent;
            Pool = new List<T>();

            for (int i = 0; i < prewarmObjects; i++)
            {
                var obj = _factory.Create(_parent);
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
            var obj = _factory.Create(_parent);
            Pool.Add(obj);
            return obj;
        }
    }
}