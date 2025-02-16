using UnityEngine;

namespace _Project.Scripts.Configs
{
    public abstract class ProjectileConfig : ScriptableObject
    {
        [field: SerializeField] public float Speed { get; private set; }
    }
}