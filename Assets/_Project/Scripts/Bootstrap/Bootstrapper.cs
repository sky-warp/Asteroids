using _Project.Scripts.AudioSystems;
using _Project.Scripts.Firebase;
using _Project.Scripts.GameOverServices;
using _Project.Scripts.LocalAssetLoaders;
using _Project.Scripts.ParticleSystems;
using _Project.Scripts.Projectiles.ProjectileTypes;
using _Project.Scripts.SaveSystems;
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
            _resourcesLoader.GetAsset(await _localAssetLoader.LoadAsset<Laser>("Laser"), out _resourcesLoader.Laser);
            _resourcesLoader.GetAsset(await _localAssetLoader.LoadAsset<Bullet>("Bullet"), out _resourcesLoader.Bullet);

            UnityEngine.SceneManagement.SceneManager.LoadScene("Main");
        }
    }
}