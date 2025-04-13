using System;
using R3;
using Zenject;

namespace _Project.Scripts.UnityAds
{
    public class AdsController : IInitializable, IDisposable
    {
        private AdManager _adManager;
        private AdsView _adsView;
        private readonly CompositeDisposable _disposable = new();

        public AdsController(AdsView adsView, AdManager adManager)
        {
            _adManager = adManager;
            _adsView = adsView;
        }


        public void Initialize()
        {
            _adsView.ShowRewardedAdButton.OnClickAsObservable()
                .Subscribe(_ => _adManager.ShowRewardAd())
                .AddTo(_disposable);

            _adManager.RewardAd.WasWatched
                .Where(value => value == true)
                .Subscribe(_ => _adsView.ShowRewardedAdButton.interactable = false)
                .AddTo(_disposable);
        }

        public void Dispose()
        {
            _disposable?.Dispose();
        }
    }
}