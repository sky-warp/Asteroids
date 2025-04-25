using R3;

namespace _Project.Scripts.InAppPurchase
{
    public interface InAppPurchaseBuyable
    {
        public ReactiveProperty<bool> IsNoAds { get; }
        void PurchaseNoAds();
    }
}