using _Project.Scripts.AudioSystems;
using _Project.Scripts.Environment.Units;
using _Project.Scripts.Firebase;
using _Project.Scripts.GameOverServices;
using _Project.Scripts.LocalAssetLoaders;
using _Project.Scripts.ParticleSystems;
using _Project.Scripts.Projectiles.ProjectileTypes;
using _Project.Scripts.SaveSystems;
using _Project.Scripts.Spaceship.View;
using _Project.Scripts.UnityAds;
using Zenject;

namespace _Project.Scripts.Bootstrap
{
    public class Bootstrapper : IInitializable
    {
        private IAssetLoadable _assetLoader;
        private MainLevelResources _mainLevelResources;

        public Bootstrapper(ScoreSaveSystem scoreSaveSystem,
            FirebaseInstaller firebaseInstaller,
            DefaultAudioManager defaultAudioManager,
            DefaultVisualEffectSystem defaultVisualEffectSystem,
            DefaultGameStateService gameStateService,
            IAssetLoadable assetLoader,
            MainLevelResources mainLevelResources,
            AdsInitializer adsInitializer,
            FullscreenUnityAdUnit fullscreenUnityAdUnit)
        {
            _assetLoader = assetLoader;
            _mainLevelResources = mainLevelResources;
        }

        public async void Initialize()
        {
            _mainLevelResources.Laser = await _assetLoader.LoadAsset<Laser>("Laser");
            _mainLevelResources.Bullet = await _assetLoader.LoadAsset<Bullet>("Bullet");
            _mainLevelResources.AsteroidBig = await _assetLoader.LoadAsset<AsteroidBig>("AsteroidBig");
            _mainLevelResources.AsteroidSmall = await _assetLoader.LoadAsset<AsteroidSmall>("AsteroidSmall");
            _mainLevelResources.UfoChaser = await _assetLoader.LoadAsset<UfoChaser>("UfoChaser");
            _mainLevelResources.Spaceship = await _assetLoader.LoadAsset<SpaceshipView>("Spaceship");

            _assetLoader.UnloadAsset();
            
            UnityEngine.SceneManagement.SceneManager.LoadScene("Main");
        }
    }
}