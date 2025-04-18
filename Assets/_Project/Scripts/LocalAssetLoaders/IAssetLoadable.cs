using _Project.Scripts.Environment.Units;
using _Project.Scripts.Projectiles.ProjectileTypes;
using _Project.Scripts.Spaceship.View;
using Cysharp.Threading.Tasks;

namespace _Project.Scripts.LocalAssetLoaders
{
    public interface IAssetLoadable
    {
        UniTask<Bullet> LoadBullet();
        UniTask<Laser> LoadLaser();
        UniTask<AsteroidBig> LoadAsteroidBig();
        UniTask<AsteroidSmall> LoadAsteroidSmall();
        UniTask<UfoChaser> LoadUfoChaser();
        UniTask<SpaceshipView> LoadSpaceship();
        void UnloadAsset();
    }
}