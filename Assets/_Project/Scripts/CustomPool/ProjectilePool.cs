using System.Collections.Generic;
using _Project.Scripts.Factories;
using _Project.Scripts.Projectiles.ProjectileTypes;
using UnityEngine;

namespace _Project.Scripts.CustomPool
{
    public class ProjectilePool<T> : BasePool<T> where T : Projectile
    {
        private Transform _parent;
        private BaseMonoFactory<T> _factory;
        private List<T> Pool { get; }

        public ProjectilePool(int prewarmObjects, Transform parent, BaseMonoFactory<T> factory) : base(prewarmObjects,
            parent, factory)
        {
        }
    }
}