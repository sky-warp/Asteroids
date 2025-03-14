using _Project.Scripts.Configs.AmmoConfigs;
using _Project.Scripts.Configs.SpaceshipConfigs;
using _Project.Scripts.Environment.EnvironmentUnitTypes;
using _Project.Scripts.Projectiles.ProjectileTypes;
using _Project.Scripts.Spaceship.View;
using UnityEngine;

namespace _Project.Scripts.Configs.GameConfigs
{
    [CreateAssetMenu(fileName = "GameConfig", menuName = "Create New Game Config/ Game Config")]
    public class GameConfig : ScriptableObject
    {
        [field: SerializeField] public SpaceshipConfig SpaceshipConfig { get; private set; }
        [field: SerializeField] public AmmoConfig AmmoConfig { get; private set; }
        [field: SerializeField] public SpaceshipView SpaceshipViewPrefab { get; private set; }
        
        [field: Header("Projectiles prefabs")]
        [field: SerializeField] public Bullet BulletPrefab { get; private set; }
        [field: SerializeField] public Laser LaserPrefab;
        
        [field: Header("Environment objects prefabs")]
        [field: SerializeField] public AsteroidBig AsteroidBigPrefab { get; private set; }
        [field: SerializeField] public AsteroidSmall AsteroidSmallPrefab { get; private set; }
        [field: SerializeField] public UfoChaser UfoChaserPrefab { get; private set; }
    }
}