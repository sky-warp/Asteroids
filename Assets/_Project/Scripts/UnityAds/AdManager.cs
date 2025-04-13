using Zenject;

namespace _Project.Scripts.UnityAds
{
    public class AdManager : IInitializable
    {
        public RewardAd RewardAd { get; private set; }
        
        private AdsInitializer _initializer;
        
        public AdManager(AdsInitializer adsInitializer, RewardAd rewardAd)
        {
            _initializer = adsInitializer;
            RewardAd = rewardAd;
        }

        public void Initialize()
        {
            _initializer.InitializeAds();
        }

        public void ShowRewardAd()
        {
            RewardAd.ShowRewardAd();
        }
    }
}