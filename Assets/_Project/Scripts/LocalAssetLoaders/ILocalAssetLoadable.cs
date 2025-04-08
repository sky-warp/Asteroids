using System.Threading.Tasks;

namespace _Project.Scripts.LocalAssetLoaders
{
    public interface ILocalAssetLoadable
    {
        Task<T> LoadAsset<T>(string key);
        void UnloadAsset();
    }
}