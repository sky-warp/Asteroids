using R3;

namespace _Project.Scripts.InAppPurchase
{
    public interface IAppPurchaseBuyable
    {
        public ReactiveProperty<bool> IsNoAds { get; }
        public ReactiveProperty<bool> IsContinue { get; }
        void PurchaseNoAds();
        void PurchaseContinueGame();
    }
}