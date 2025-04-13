using Zenject;

namespace _Project.Scripts.UnityAds
{
    public class AdManager : IInitializable
    {
        public RewardAd RewardAd { get; private set; }
        public ShortAd ShortAd { get; private set; }
        
        private AdsInitializer _initializer;
        
        public AdManager(AdsInitializer adsInitializer)
        {
            _initializer = adsInitializer;
            RewardAd = adsInitializer.RewardAd;
            ShortAd = adsInitializer.ShortAd;
        }

        public void Initialize()
        {
            _initializer.InitializeAds();
        }

        public void ShowRewardAd()
        {
            RewardAd.ShowRewardAd();
        }

        public void ShowShortAd()
        {
            ShortAd.ShowShortAd();
        }
    }
}