using Cysharp.Threading.Tasks;

namespace _Project.Scripts.SaveSystems
{
    public interface ISaveable
    {
        public SaveData SavedData { get; }
        
        UniTask Init();
        void InitializeLocalSave();
        UniTask InitializeRemoteSave();
        void ResetHighScore();
        void ResetNoAdsPurchased();
        bool CheckBuyStatus();
    }
}