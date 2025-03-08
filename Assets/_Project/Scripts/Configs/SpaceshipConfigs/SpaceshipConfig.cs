using UnityEngine;

namespace _Project.Scripts.Configs.SpaceshipConfigs
{
    [CreateAssetMenu(fileName = "SpaceshipConfig", menuName = "Create New Spaceship Config/Spaceship Config")]
    public class SpaceshipConfig : ScriptableObject
    {
        [field: SerializeField] public float Speed { get; private set; }
        [field: SerializeField] public float LaserCooldown { get; private set; }
    }
}