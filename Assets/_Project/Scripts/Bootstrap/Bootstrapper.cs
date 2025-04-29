using _Project.Scripts.Firebase;
using _Project.Scripts.InAppPurchase;
using _Project.Scripts.LocalAssetLoaders;
using _Project.Scripts.SaveSystems;
using _Project.Scripts.SceneManagers;
using Cysharp.Threading.Tasks;
using Unity.Services.Core;
using Zenject;

namespace _Project.Scripts.Bootstrap
{
    public class Bootstrapper : IInitializable
    {
        private IAssetLoadable _assetLoader;
        private IRemoteDataLoadable _remoteConfigInitializer;
        private MainLevelResources _mainLevelResources;
        private SceneManager _sceneManager;
        private ISaveable _saveSystem;
        private IAPInitializer _initializer;

        public Bootstrapper(IAssetLoadable assetLoader,
            MainLevelResources mainLevelResources,
            SceneManager sceneManager,
            IRemoteDataLoadable remoteConfigInitializer,
            ISaveable saveSystem,
            IAPInitializer initializer
        )
        {
            _assetLoader = assetLoader;
            _mainLevelResources = mainLevelResources;
            _sceneManager = sceneManager;
            _remoteConfigInitializer = remoteConfigInitializer;
            _saveSystem = saveSystem;
            _initializer = initializer;
        }

        public async void Initialize()
        {
            await UnityServices.InitializeAsync();

            var (laser, bullet, asteroidBig, asteroidSmall, ufoChaser, spaceship) = await UniTask.WhenAll(
                _assetLoader.LoadLaser(),
                _assetLoader.LoadBullet(),
                _assetLoader.LoadAsteroidBig(),
                _assetLoader.LoadAsteroidSmall(),
                _assetLoader.LoadUfoChaser(),
                _assetLoader.LoadSpaceship()
            );

            _mainLevelResources.Laser = laser;
            _mainLevelResources.Bullet = bullet;
            _mainLevelResources.AsteroidBig = asteroidBig;
            _mainLevelResources.AsteroidSmall = asteroidSmall;
            _mainLevelResources.UfoChaser = ufoChaser;
            _mainLevelResources.Spaceship = spaceship;

            _assetLoader.UnloadAsset();

            await _remoteConfigInitializer.LoadRemoteData();

            await _saveSystem.Init();
            
            _initializer.Init();

            _sceneManager.LoadMainScene();
        }
    }
}