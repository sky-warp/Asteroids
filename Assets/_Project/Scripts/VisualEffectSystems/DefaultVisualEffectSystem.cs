using _Project.Scripts.Factories;
using _Project.Scripts.VisualEffectSystems.ParticleTypes;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.ParticleSystems
{
    public class DefaultVisualEffectSystem : MonoBehaviour
    {
        private Transform _projectileVisualEffectsParent;

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

        public void CreateProjectileVisualEffects(Transform target)
        {
            _projectileVisualEffectsParent = target;

            _shootParticle = _bulletShootFactory.CreateVisualEffect(transform);
        }

        public void CreateUnitEffects(Transform target)
        {
            _unitDestroyParticle = _unitDestroyFactory.CreateVisualEffect(transform);
        }

    public void PlayGunShootEffect()
        {
            _shootParticle.transform.position = _projectileVisualEffectsParent.position;
            _shootParticle.transform.rotation = _projectileVisualEffectsParent.rotation;
            _shootParticle.Play();
        }

        public void PlayUnitDestroyEffect(Vector3 targetPosition)
        {
            _unitDestroyParticle.transform.position = targetPosition;
            _unitDestroyParticle.Play();
        }
    }
}