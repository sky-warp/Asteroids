using R3;

namespace _Project.Scripts.UnityAds
{
    public interface IAdShowable
    {
        ReactiveProperty<bool> RewardAdWasWatched { get; }
        ReactiveProperty<bool> ShortAdWasWatched { get; }

        public RewardAd RewardAd { get;}
        public ShortAd ShortAd { get;}
        
        void ShowRewardAd();
        void ShowShortAd();
    }
}