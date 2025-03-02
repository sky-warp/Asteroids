using UnityEngine;

namespace _Project.Scripts.Configs.EnvironmentConfigs
{
    [CreateAssetMenu (fileName = "EnvironmentUnitConfig", menuName = "Create New Environment Unit Config")]
    public class EnvironmentUnitConfig : ScriptableObject
    {
        [field: SerializeField] public float UnitSpeed { get; private set; }
        [field: SerializeField] public int UnitScore { get; private set; }
    }
}