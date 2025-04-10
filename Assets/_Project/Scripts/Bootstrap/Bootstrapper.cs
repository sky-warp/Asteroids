using _Project.Scripts.Environment.Units;
using _Project.Scripts.LocalAssetLoaders;
using _Project.Scripts.Projectiles.ProjectileTypes;
using _Project.Scripts.SceneManagers;
using _Project.Scripts.Spaceship.View;
using Cysharp.Threading.Tasks;
using Zenject;

namespace _Project.Scripts.Bootstrap
{
    public class Bootstrapper : IInitializable
    {
        private IAssetLoadable _assetLoader;
        private MainLevelResources _mainLevelResources;
        private SceneManager _sceneManager;

        public Bootstrapper(IAssetLoadable assetLoader,
            MainLevelResources mainLevelResources,
            SceneManager sceneManager)
        {
            _assetLoader = assetLoader;
            _mainLevelResources = mainLevelResources;
            _sceneManager = sceneManager;
        }

        public async void Initialize()
        {
            var (laser, bullet, asteroidBig, asteroidSmall, ufoChaser, spaceship) = await UniTask.WhenAll(
                _assetLoader.LoadLaser<Laser>("Laser"),
                _assetLoader.LoadBullet<Bullet>("Bullet"),
                _assetLoader.LoadAsteroidBig<AsteroidBig>("AsteroidBig"),
                _assetLoader.LoadAsteroidSmall<AsteroidSmall>("AsteroidSmall"),
                _assetLoader.LoadUfoChaser<UfoChaser>("UfoChaser"),
                _assetLoader.LoadSpaceship<SpaceshipView>("Spaceship")
            );

            _mainLevelResources.Laser = laser;
            _mainLevelResources.Bullet = bullet;
            _mainLevelResources.AsteroidBig = asteroidBig;
            _mainLevelResources.AsteroidSmall = asteroidSmall;
            _mainLevelResources.UfoChaser = ufoChaser;
            _mainLevelResources.Spaceship = spaceship;

            _assetLoader.UnloadAsset();

            _sceneManager.LoadMainScene();
        }
    }
}