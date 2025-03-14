using _Project.Scripts.Configs.GameConfigs;
using _Project.Scripts.Environment.EnvironmentUnitTypes;
using _Project.Scripts.Projectiles.ProjectileTypes;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Installers
{
    [CreateAssetMenu(fileName = "ProjectInstaller", menuName = "Installers/ProjectInstaller")]
    public class ProjectInstaller : ScriptableObjectInstaller
    {
        [SerializeField] private GameConfig _gameConfig;

        public override void InstallBindings()
        {
            Container
                .Bind<GameConfig>()
                .FromInstance(_gameConfig)
                .AsSingle();

            Container
                .Bind<CoroutineManager.CoroutineManager>()
                .FromNewComponentOnNewGameObject()
                .AsSingle();

            Container
                .Bind<AsteroidBig>()
                .FromInstance(_gameConfig.AsteroidBigPrefab)
                .AsSingle();

            Container
                .Bind<AsteroidSmall>()
                .FromInstance(_gameConfig.AsteroidSmallPrefab)
                .AsSingle();

            Container
                .Bind<UfoChaser>()
                .FromInstance(_gameConfig.UfoChaserPrefab)
                .AsSingle();
            
            Container
                .Bind<Bullet>()
                .FromInstance(_gameConfig.BulletPrefab)
                .AsSingle();

            Container
                .Bind<Laser>()
                .FromInstance(_gameConfig.LaserPrefab)
                .AsSingle();
        }
    }
}