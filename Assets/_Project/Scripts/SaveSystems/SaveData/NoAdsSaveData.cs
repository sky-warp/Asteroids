using System;

namespace _Project.Scripts.SaveSystems
{
    [Serializable]
    public class NoAdsSaveData
    {
        public int PurchaseStatus;
        public event Action WasPurchased;

        private bool _isBuyed;

        public void SetPurchaseStatus(int purchaseStatus)
        {
            PurchaseStatus = purchaseStatus;

            if (purchaseStatus == 1)
                WasPurchased?.Invoke();
        }

        public void SetDefaultStatus()
        {
            PurchaseStatus = 0;
        }

        public bool CheckStatus()
        {
            if (PurchaseStatus == 0)
            {
                _isBuyed = false;
                return _isBuyed;
            }

            _isBuyed = true;
            return _isBuyed;
        }
    }
}