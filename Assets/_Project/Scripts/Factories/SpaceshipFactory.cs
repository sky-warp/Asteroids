using _Project.Scripts.Spaceship.View;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Factories
{
    public class SpaceshipFactory<T> : BaseMonoFactory<T> where T : SpaceshipView
    {
        private T _spaceship;
        private readonly DiContainer _container;
        
        public SpaceshipFactory(T monoPrefab, DiContainer container) : base(monoPrefab)
        {
            _spaceship = monoPrefab;
            _container = container;
        }

        public override T Create(Transform parent)
        {
            var spaceship = _container.InstantiatePrefabForComponent<T>(_spaceship);
            
            _container.Inject(spaceship);
            
            return spaceship;
        }
    }
}