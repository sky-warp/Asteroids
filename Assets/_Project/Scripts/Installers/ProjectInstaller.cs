using _Project.Scripts.AudioSystems;
using _Project.Scripts.AudioSystems.AudioTypes;
using _Project.Scripts.Configs.Ads;
using _Project.Scripts.Configs.AudioConfigs;
using _Project.Scripts.Configs.ParticleConfigs;
using _Project.Scripts.CoroutineManagers;
using _Project.Scripts.Factories;
using _Project.Scripts.Firebase;
using _Project.Scripts.GameOverServices;
using _Project.Scripts.LocalAssetLoaders;
using _Project.Scripts.ParticleSystems;
using _Project.Scripts.SaveSystems;
using _Project.Scripts.SceneManagers;
using _Project.Scripts.UnityAds;
using _Project.Scripts.VisualEffectSystems.ParticleTypes;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Installers
{
    [CreateAssetMenu(fileName = "ProjectInstaller", menuName = "Installers/ProjectInstaller")]
    public class ProjectInstaller : ScriptableObjectInstaller
    {
        [SerializeField] private AudioSystemConfig _audioSystemConfig;
        [SerializeField] private VisualEffectsConfig _visualEffectsConfig;
        [SerializeField] private AdsConfig _adsConfig;

        public override void InstallBindings()
        {
            Container
                .BindInterfacesAndSelfTo<RemoteConfigInitializer>()
                .AsSingle();

            Container
                .Bind<RemoteData>()
                .AsSingle();
            
            Container
                .BindInterfacesAndSelfTo<AdManager>()
                .AsSingle()
                .NonLazy();

            Container
                .Bind<AdsInitializer>()
                .AsSingle()
                .WithArguments(_adsConfig);

            Container
                .BindInterfacesAndSelfTo<RewardAd>()
                .AsSingle()
                .WithArguments(_adsConfig);

            Container
                .BindInterfacesAndSelfTo<ShortAd>()
                .AsSingle()
                .WithArguments(_adsConfig);

            Container
                .Bind<IAssetLoadable>()
                .To<AssetLoader>()
                .AsSingle();

            Container
                .Bind<MainLevelResources>()
                .AsSingle();

            Container
                .Bind<DefaultGameStateService>()
                .AsSingle()
                .NonLazy();

            Container
                .Bind<CoroutineManager>()
                .FromNewComponentOnNewGameObject()
                .AsSingle();

            Container
                .BindInterfacesAndSelfTo<ScoreSaveSystem>()
                .AsSingle()
                .NonLazy();

            Container
                .Bind<FirebaseInstaller>()
                .FromNewComponentOnNewGameObject()
                .AsSingle()
                .NonLazy();

            Container
                .Bind<FirebaseEventManager>()
                .FromNewComponentOnNewGameObject()
                .AsSingle();

            Container
                .Bind<SaveData>()
                .AsSingle();

            Container
                .Bind<SceneManager>()
                .AsSingle();

            Container
                .Bind<DefaultAudioManager>()
                .FromNewComponentOnNewGameObject()
                .AsSingle()
                .WithArguments(
                    new SoundSourceFactory<BulletSoundSource>(new BulletSoundSource(_audioSystemConfig.BulletSound)),
                    new SoundSourceFactory<LaserSoundSource>(new LaserSoundSource(_audioSystemConfig.LaserSound)),
                    new SoundSourceFactory<ScoreEarnSoundSource>(
                        new ScoreEarnSoundSource(_audioSystemConfig.ScoreEarnScound)),
                    new SoundSourceFactory<BackgroundMusic>(new BackgroundMusic(_audioSystemConfig.BackgroundMusic)))
                .NonLazy();

            Container
                .Bind<DefaultVisualEffectSystem>()
                .FromNewComponentOnNewGameObject()
                .AsSingle()
                .WithArguments(
                    new VisualEffectFactory<BulletShootEffect>(new BulletShootEffect(_visualEffectsConfig.ShootEffect)),
                    new VisualEffectFactory<UnitDestroyEffect>(
                        new UnitDestroyEffect(_visualEffectsConfig.UnitDestroyEffect)))
                .NonLazy();
        }
    }
}