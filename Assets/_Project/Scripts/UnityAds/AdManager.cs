using System;
using R3;
using Zenject;

namespace _Project.Scripts.UnityAds
{
    public class AdManager : IAdShowable, IInitializable, IDisposable
    {
        public ReactiveProperty<bool> RewardAdWasWatched { get; } = new();
        public ReactiveProperty<bool> ShortAdWasWatched { get; } = new();

        public RewardAd RewardAd { get;}
        public ShortAd ShortAd { get;}

        private AdsInitializer _initializer;

        private readonly CompositeDisposable _disposable = new();
        private ReactiveProperty<bool> _rewardAdWasWatched;
        private ReactiveProperty<bool> _shortAdWasWatched;
        private ReactiveProperty<bool> _rewardAdWasWatched1;
        private ReactiveProperty<bool> _shortAdWasWatched1;

        public AdManager(AdsInitializer adsInitializer)
        {
            _initializer = adsInitializer;
            RewardAd = adsInitializer.RewardAd;
            ShortAd = adsInitializer.ShortAd;
        }

        public void Initialize()
        {
            RewardAd.WasWatched
                .Subscribe(x => RewardAdWasWatched.Value = x)
                .AddTo(_disposable);

            ShortAd.WasWatched
                .Subscribe(x => ShortAdWasWatched.Value = x)
                .AddTo(_disposable);

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

        public void Dispose()
        {
            _disposable?.Dispose();
        }
    }
}