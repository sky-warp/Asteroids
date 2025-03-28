using UnityEngine;

namespace _Project.Scripts.VisualEffectSystems.ParticleTypes
{
    public abstract class VisualEffect
    {
        public readonly ParticleSystem Effect;
        
        protected VisualEffect(ParticleSystem effect)
        {
            Effect = effect;
        }
    }
}