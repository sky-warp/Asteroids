namespace _Project.Scripts.LocalAssetLoaders
{
    public interface IGetDownloadedAssetable
    {
        void GetAsset<T>(T loadedAsset, out T originalAsset);
    }
}