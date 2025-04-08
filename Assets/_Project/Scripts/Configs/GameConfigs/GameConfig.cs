using _Project.Scripts.Configs.AmmoConfigs;
using _Project.Scripts.Configs.SpaceshipConfigs;
using _Project.Scripts.Environment.Units;
using _Project.Scripts.Spaceship.View;
using UnityEngine;

namespace _Project.Scripts.Configs.GameConfigs
{
    [CreateAssetMenu(fileName = "GameConfig", menuName = "Create New Game Config/ Game Config")]
    public class GameConfig : ScriptableObject
    {
        [field: SerializeField] public SpaceshipConfig SpaceshipConfig { get; private set; }
        [field: SerializeField] public AmmoConfig AmmoConfig { get; private set; }
    }
}