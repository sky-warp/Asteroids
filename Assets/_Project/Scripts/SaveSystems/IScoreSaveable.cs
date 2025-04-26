namespace _Project.Scripts.SaveSystems
{
    public interface ISaveable
    {
        public ScoreSaveData ScoreSaveData { get; }
        
        void Init();
        void InitializeLocalSave();
        void InitializeRemoteSave();
        void ResetParticularSaveData();
    }
}