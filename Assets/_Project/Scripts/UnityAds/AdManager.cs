using Zenject;

namespace _Project.Scripts.UnityAds
{
    public class AdManager : IInitializable
    {
        private AdsInitializer _initializer;
        private RewardAd _rewardAd;
        
        public AdManager(AdsInitializer adsInitializer, RewardAd rewardAd)
        {
            _initializer = adsInitializer;
            _rewardAd = rewardAd;
        }

        public void Initialize()
        {
            _initializer.InitializeAds();
        }

        public void ShowRewardAd()
        {
            _rewardAd.ShowRewardAd();
        }
    }
}