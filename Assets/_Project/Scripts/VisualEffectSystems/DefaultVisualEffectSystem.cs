using _Project.Scripts.Factories;
using _Project.Scripts.VisualEffectSystems.ParticleTypes;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.ParticleSystems
{
    public class DefaultVisualEffectSystem : MonoBehaviour
    {
        private Transform _visualEffectsParent;
        
        private ParticleSystem _shootParticle;
        private ParticleSystem _unitDestroyParticle;

        private VisualEffectFactory<BulletShootEffect> _bulletShootFactory;
        private VisualEffectFactory<UnitDestroyEffect> _unitDestroyFactory;

        [Inject]
        private void Construct(VisualEffectFactory<BulletShootEffect> bulletShootFactory,
            VisualEffectFactory<UnitDestroyEffect> unitDestroyFactory)
        {
            _bulletShootFactory = bulletShootFactory;
            _unitDestroyFactory = unitDestroyFactory;
        }

        public void CreateVisualEffects(Transform target)
        {
            _visualEffectsParent = target;
            
            _shootParticle = _bulletShootFactory.CreateVisualEffect(transform);
            _unitDestroyParticle = _unitDestroyFactory.CreateVisualEffect(transform);
        }

        public void PlayGunShootEffect()
        {
            _shootParticle.transform.position = _visualEffectsParent.position;
            _shootParticle.transform.rotation = _visualEffectsParent.rotation;
            _shootParticle.Play();
        }

        public void PlayUnitDestroyEffect(Transform targetPosition)
        {
            _unitDestroyParticle.transform.position = targetPosition.position;
            _unitDestroyParticle.Play();
        }
    }
}