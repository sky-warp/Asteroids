using System.Collections.Generic;
using _Project.Scripts.Environment;
using _Project.Scripts.Factories;
using UnityEngine;

namespace _Project.Scripts.CustomPool
{
    public class EnvironmentUnitPool<T> : BasePool<T> where T : EnvironmentObject
    {
        private Transform _parent;
        private BaseMonoFactory<T> _factory;
        private List<T> Pool { get; }

        public EnvironmentUnitPool(int prewarmObjects, Transform parent, BaseMonoFactory<T> factory) : base(
            prewarmObjects, parent, factory)
        {
        }
    }
}