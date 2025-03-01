using UnityEngine;

namespace _Project.Scripts.Configs.AmmoConfig
{
    [CreateAssetMenu(fileName = "AmmoConfig", menuName = "Create New Ammo Config/ AmmoConfig")]
    public class AmmoConfig : ScriptableObject
    {
        [field: SerializeField] public int LaserCount {get; private set;} 
        [field: SerializeField] public float LaserCooldown {get; private set;} 
    }
}