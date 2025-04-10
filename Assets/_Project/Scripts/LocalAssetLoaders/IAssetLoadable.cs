using Cysharp.Threading.Tasks;

namespace _Project.Scripts.LocalAssetLoaders
{
    public interface IAssetLoadable
    {
        UniTask<Bullet> LoadBullet<Bullet>(string key);
        UniTask<Laser> LoadLaser<Laser>(string key);
        UniTask<AsteroidBig> LoadAsteroidBig<AsteroidBig>(string key);
        UniTask<AsteroidSmall> LoadAsteroidSmall<AsteroidSmall>(string key);
        UniTask<UfoChaser> LoadUfoChaser<UfoChaser>(string key);
        UniTask<SpaceshipView> LoadSpaceship<SpaceshipView>(string key);
        void UnloadAsset();
    }
}