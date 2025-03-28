using UnityEngine;

namespace _Project.Scripts.Configs.ParticleConfigs
{
    [CreateAssetMenu(fileName = "VisualEffectsConfig", menuName = "Create New Visual Effects Config/ParticleConfig")]
    public class VisualEffectsConfig : ScriptableObject
    {
        [field: SerializeField] public ParticleSystem ShootEffect { get; private set; }
        [field: SerializeField] public ParticleSystem UnitDestroyEffect { get; private set; }
    }
}