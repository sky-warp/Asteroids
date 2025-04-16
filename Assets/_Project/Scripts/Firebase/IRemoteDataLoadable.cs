using Cysharp.Threading.Tasks;

namespace _Project.Scripts.Firebase
{
    public interface IRemoteDataLoadable
    {
        UniTask LoadRemoteData();
    }
}