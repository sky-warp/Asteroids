using _Project.Scripts.AudioSystems;
using _Project.Scripts.Firebase;
using _Project.Scripts.GameOverServices;
using _Project.Scripts.LocalAssetLoaders;
using _Project.Scripts.ParticleSystems;
using _Project.Scripts.SaveSystems;
using Zenject;

namespace _Project.Scripts.Bootstrap
{
    public class Bootstrapper : IInitializable
    {
        private ResourcesLoader _resourcesLoader;
        
        public Bootstrapper(ScoreSaveSystem scoreSaveSystem,
            FirebaseInstaller firebaseInstaller,
            DefaultAudioManager defaultAudioManager,
            DefaultVisualEffectSystem defaultVisualEffectSystem,
            DefaultGameStateService gameStateService,
            LocalAssetLoader localAssetLoader,
            ResourcesLoader resourcesLoader)
        {
            _resourcesLoader = resourcesLoader;
        }

        public async void Initialize()
        {
            _resourcesLoader.Laser = await _resourcesLoader.LocalAssetLoader.LoadAssets("Laser");
            
            UnityEngine.SceneManagement.SceneManager.LoadScene("Main");
        }
    }
}