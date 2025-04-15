using _Project.Scripts.Projectiles.ProjectileTypes;
using UnityEngine;

namespace _Project.Scripts.Factories
{
    public class ProjectileFactory<T> where T : Projectile
    {
        private T _projectile;
        private float _speed;

        public ProjectileFactory(T projectile, float speed)
        {
            _projectile = projectile;
            _speed = speed;
        }

        public T Create(Transform parent)
        {
            var projectileInstance = GameObject.Instantiate(_projectile, parent);
            projectileInstance.SetSpeed(_speed);
            return projectileInstance;
        }
    }
}