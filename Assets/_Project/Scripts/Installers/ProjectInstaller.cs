using _Project.Scripts.Configs.GameConfigs;
using _Project.Scripts.Configs.SpawnerConfigs;
using _Project.Scripts.CoroutineManagers;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Installers
{
    [CreateAssetMenu(fileName = "ProjectInstaller", menuName = "Installers/ProjectInstaller")]
    public class ProjectInstaller : ScriptableObjectInstaller
    {
        [SerializeField] private GameConfig _gameConfig;
        [SerializeField] private SpawnerConfig _spawnerConfig;

        public override void InstallBindings()
        {
            Container
                .Bind<GameConfig>()
                .FromInstance(_gameConfig)
                .AsSingle();

            Container
                .Bind<SpawnerConfig>()
                .FromInstance(_spawnerConfig)
                .AsSingle();
            
            Container
                .Bind<CoroutineManager>()
                .FromNewComponentOnNewGameObject()
                .AsSingle();
        }
    }
}