using System;

namespace _Project.Scripts.SaveSystems
{
    [Serializable]
    public class SaveData
    {
        public int HighScore;
        public int NoAdsPurchaseStatus;
        public string LastSaveDate;
        public string LastSaveTime;

        public bool IsContinueAvailable { get; private set; }

        public event Action OnHighScoreChanged;
        public event Action OnHighScoreReset;
        public event Action OnNoAdsPurchased;
        public event Action OnNoAdsReseted;

        private bool _isNoAdsPurchase;

        public int SetHighScoreData(int highScore)
        {
            if (highScore > HighScore)
            {
                HighScore = highScore;

                OnHighScoreChanged?.Invoke();
            }

            return HighScore;
        }

        public void ClearHighScoreData()
        {
            HighScore = 0;
            OnHighScoreReset?.Invoke();
        }

        public void SetDefaultValues()
        {
            HighScore = 0;
            NoAdsPurchaseStatus = 0;

            LastSaveDate = DateTime.Now.ToShortDateString();
            LastSaveTime = DateTime.Now.ToLongTimeString();
        }

        public void SetPurchaseStatus(int purchaseStatus)
        {
            NoAdsPurchaseStatus = purchaseStatus;

            if (purchaseStatus == 1)
                OnNoAdsPurchased?.Invoke();
        }

        public void ResetPurchaseStatus()
        {
            NoAdsPurchaseStatus = 0;
            OnNoAdsReseted?.Invoke();
        }

        public void SetOnGameContinueStatus(bool isAvaliable)
        {
            IsContinueAvailable = isAvaliable;
        }

        public bool CheckStatus()
        {
            if (NoAdsPurchaseStatus == 0)
            {
                _isNoAdsPurchase = false;
                return _isNoAdsPurchase;
            }

            _isNoAdsPurchase = true;
            return _isNoAdsPurchase;
        }

        public void SaveDate(string date)
        {
            LastSaveDate = date;
        }

        public void SaveTime(string time)
        {
            LastSaveTime = time;
        }
    }
}