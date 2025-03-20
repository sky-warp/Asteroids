using UnityEngine;

namespace _Project.Scripts.Configs.SpawnerConfigs
{
    [CreateAssetMenu(fileName = "SpawnerConfig", menuName = "Create New Spawn Config/SpawnerConfig")]
    public class SpawnerConfig : ScriptableObject
    {
        [field: SerializeField] public float MinSpawnSpace { get; private set; }
        [field: SerializeField] public float MaxSpawnSpace { get; private set; }
    }

    public enum SidesToSpawn
    {
        Left,
        Right,
        Top,
        Bottom
    }
}