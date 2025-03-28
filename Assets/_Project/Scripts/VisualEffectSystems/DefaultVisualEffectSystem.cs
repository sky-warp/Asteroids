using _Project.Scripts.Factories;
using _Project.Scripts.VisualEffectSystems.ParticleTypes;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.ParticleSystems
{
    public class DefaultVisualEffectSystem : MonoBehaviour
    {
        private ParticleSystem _shootParticle;

        [Inject]
        private void Construct(VisualEffectFactory<BulletShootEffect> bulletShootFactory)
        {
            _shootParticle = bulletShootFactory.CreateVisualEffect(transform);
        }

        public void CreateBulletShootEffect()
        {
            _shootParticle.Play();
        }
    }
}