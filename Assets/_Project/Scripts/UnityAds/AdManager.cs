using System;
using R3;
using Zenject;

namespace _Project.Scripts.UnityAds
{
    public class AdManager : IInitializable, IDisposable
    {
        public readonly ReactiveProperty<bool> RewardAdWasWatched = new(false);
        public readonly ReactiveProperty<bool> ShortAdWasWatched = new(false);
        
        public RewardAd RewardAd { get; private set; }
        public ShortAd ShortAd { get; private set; }
        
        private AdsInitializer _initializer;
        
        private readonly CompositeDisposable _disposable = new();
        
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