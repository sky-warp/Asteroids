using _Project.Scripts.Projectiles.ProjectileTypes;

namespace _Project.Scripts.LocalAssetLoaders
{
    public class ResourcesLoader : IGetDownloadedAssetable
    {
        public Laser Laser;
        public Bullet Bullet;

        public void GetAsset<T>(T loadedAsset, out T originalAsset)
        {
            originalAsset = loadedAsset;
        }
    }
}