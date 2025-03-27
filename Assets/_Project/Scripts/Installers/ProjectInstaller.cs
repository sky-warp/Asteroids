using _Project.Scripts.AudioSystems;
using _Project.Scripts.AudioSystems.AudioTypes;
using _Project.Scripts.Configs.AudioConfigs;
using _Project.Scripts.Configs.GameConfigs;
using _Project.Scripts.Configs.SpawnerConfigs;
using _Project.Scripts.CoroutineManagers;
using _Project.Scripts.Firebase;
using _Project.Scripts.Infrastructure;
using _Project.Scripts.SaveSystems;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Installers
{
    [CreateAssetMenu(fileName = "ProjectInstaller", menuName = "Installers/ProjectInstaller")]
    public class ProjectInstaller : ScriptableObjectInstaller
    {
        [SerializeField] private GameConfig _gameConfig;
        [SerializeField] private SpawnerConfig _spawnerConfig;
        [SerializeField] private AudioSystemConfig _audioSystemConfig;

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

            Container
                .BindInterfacesAndSelfTo<ScoreSaveSystem>()
                .AsSingle();

            Container
                .Bind<FirebaseInstaller>()
                .FromNewComponentOnNewGameObject()
                .AsSingle();
            Container
                .Bind<FirebaseEventManager>()
                .FromNewComponentOnNewGameObject()
                .AsSingle();

            Container
                .Bind<GameEventManager>()
                .AsSingle();

            Container
                .Bind<SaveData>()
                .AsSingle();

            Container
                .Bind<DefaultAudioManager>()
                .FromNewComponentOnNewGameObject()
                .AsSingle();

            Container
                .Bind<BulletSoundSource>()
                .FromMethod(_ => new BulletSoundSource(_audioSystemConfig.BulletSound))
                .AsSingle();
            Container
                .Bind<LaserSoundSource>()
                .FromMethod(_ => new LaserSoundSource(_audioSystemConfig.LaserSound))
                .AsSingle();
            Container
                .Bind<ScoreEarnSoundSource>()
                .FromMethod(_ => new ScoreEarnSoundSource(_audioSystemConfig.ScoreEarnScound))
                .AsSingle();
        }
    }
}