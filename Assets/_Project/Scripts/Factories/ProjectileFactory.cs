using _Project.Scripts.Projectiles.ProjectileTypes;
using UnityEngine;

namespace _Project.Scripts.Factories
{
    public class ProjectileFactory<T> : BaseMonoFactory<T> where T : Projectile
    {
        private T _projectile;
        private float _speed;

        public ProjectileFactory(T projectile, float speed) : base(projectile)
        {
            _projectile = projectile;
            _speed = speed;
        }

        public override T Create(Transform parent)
        {
            var projectileInstance = GameObject.Instantiate(_projectile, parent);
            projectileInstance.SetSpeed(_speed);
            return projectileInstance;
        }
    }
}