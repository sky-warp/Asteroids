using UnityEngine;

namespace _Project.Scripts.Configs.ProjectilesConfigs
{
    public abstract class ProjectileConfig : ScriptableObject
    {
        [field: SerializeField] public float Speed { get; private set; }
    }
}