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

        private VisualEffectFactory<BulletShootEffect> _bulletShootFactory;

        [Inject]
        private void Construct(VisualEffectFactory<BulletShootEffect> bulletShootFactory)
        {
            _bulletShootFactory = bulletShootFactory;
        }

        public void CreateBulletShootEffect(Transform target)
        {
            _visualEffectsParent = target;
            _shootParticle = _bulletShootFactory.CreateVisualEffect(transform);
        }

        public void PlayGunShootEffect()
        {
            _shootParticle.transform.position = _visualEffectsParent.position;
            _shootParticle.transform.rotation = _visualEffectsParent.rotation;
            _shootParticle.Play();
        }
    }
}