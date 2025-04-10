using System.Threading.Tasks;

namespace _Project.Scripts.LocalAssetLoaders
{
    public interface IAssetLoadable
    {
        Task<T> LoadAsset<T>(string key);
        void UnloadAsset();
    }
}