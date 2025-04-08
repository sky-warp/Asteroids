using _Project.Scripts.AudioSystems;
using _Project.Scripts.Environment.Units;
using _Project.Scripts.Firebase;
using _Project.Scripts.GameOverServices;
using _Project.Scripts.LocalAssetLoaders;
using _Project.Scripts.ParticleSystems;
using _Project.Scripts.Projectiles.ProjectileTypes;
using _Project.Scripts.SaveSystems;
using _Project.Scripts.Spaceship.View;
using Zenject;

namespace _Project.Scripts.Bootstrap
{
    public class Bootstrapper : IInitializable
    {
        private ILocalAssetLoadable _localAssetLoader;
        private ResourcesLoader _resourcesLoader;

        public Bootstrapper(ScoreSaveSystem scoreSaveSystem,
            FirebaseInstaller firebaseInstaller,
            DefaultAudioManager defaultAudioManager,
            DefaultVisualEffectSystem defaultVisualEffectSystem,
            DefaultGameStateService gameStateService,
            ILocalAssetLoadable localAssetLoader,
            ResourcesLoader resourcesLoader)
        {
            _localAssetLoader = localAssetLoader;
            _resourcesLoader = resourcesLoader;
        }

        public async void Initialize()
        {
            _resourcesLoader.Laser = await _localAssetLoader.LoadAsset<Laser>("Laser");
            _resourcesLoader.Bullet = await _localAssetLoader.LoadAsset<Bullet>("Bullet");
            _resourcesLoader.AsteroidBig = await _localAssetLoader.LoadAsset<AsteroidBig>("AsteroidBig");
            _resourcesLoader.AsteroidSmall = await _localAssetLoader.LoadAsset<AsteroidSmall>("AsteroidSmall");
            _resourcesLoader.UfoChaser = await _localAssetLoader.LoadAsset<UfoChaser>("UfoChaser");
            _resourcesLoader.Spaceship = await _localAssetLoader.LoadAsset<SpaceshipView>("Spaceship");

            _localAssetLoader.UnloadAsset();
            
            UnityEngine.SceneManagement.SceneManager.LoadScene("Main");
        }
    }
}