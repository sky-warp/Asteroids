using _Project.Scripts.AudioSystems;
using _Project.Scripts.AudioSystems.AudioTypes;
using _Project.Scripts.Configs.AudioConfigs;
using _Project.Scripts.Configs.GameConfigs;
using _Project.Scripts.Configs.ParticleConfigs;
using _Project.Scripts.Configs.SpawnerConfigs;
using _Project.Scripts.CoroutineManagers;
using _Project.Scripts.Factories;
using _Project.Scripts.Firebase;
using _Project.Scripts.GameOverServices;
using _Project.Scripts.ParticleSystems;
using _Project.Scripts.SaveSystems;
using _Project.Scripts.VisualEffectSystems.ParticleTypes;
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
        [SerializeField] private VisualEffectsConfig _visualEffectsConfig;

        public override void InstallBindings()
        {
            Container
                .Bind<GameConfig>()
                .FromInstance(_gameConfig)
                .AsSingle();

            Container
                .Bind<DefaultGameStateService>()
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
                .Bind<SaveData>()
                .AsSingle();

            Container
                .Bind<DefaultAudioManager>()
                .FromNewComponentOnNewGameObject()
                .AsSingle()
                .WithArguments(
                    new SoundSourceFactory<BulletSoundSource>(new BulletSoundSource(_audioSystemConfig.BulletSound)),
                    new SoundSourceFactory<LaserSoundSource>(new LaserSoundSource(_audioSystemConfig.LaserSound)),
                    new SoundSourceFactory<ScoreEarnSoundSource>(new ScoreEarnSoundSource(_audioSystemConfig.ScoreEarnScound)),
                    new SoundSourceFactory<BackgroundMusic>(new BackgroundMusic(_audioSystemConfig.BackgroundMusic)));

            Container
                .Bind<DefaultVisualEffectSystem>()
                .FromNewComponentOnNewGameObject()
                .AsSingle()
                .WithArguments(
                    new VisualEffectFactory<BulletShootEffect>(new BulletShootEffect(_visualEffectsConfig.ShootEffect)),
                    new VisualEffectFactory<UnitDestroyEffect>(new UnitDestroyEffect(_visualEffectsConfig.UnitDestroyEffect)));
        }
    }
}