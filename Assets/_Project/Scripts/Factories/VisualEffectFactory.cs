using _Project.Scripts.VisualEffectSystems.ParticleTypes;
using UnityEngine;

namespace _Project.Scripts.Factories
{
    public class VisualEffectFactory<T> where T : VisualEffect 
    {
        private T _visualEffectPrefab;

        public VisualEffectFactory(T prefab)
        {
            _visualEffectPrefab = prefab;
        }

        public ParticleSystem CreateVisualEffect(Transform parent)
        {
            var instance = GameObject.Instantiate(_visualEffectPrefab.Effect, parent);
            return instance;
        }
    }
}