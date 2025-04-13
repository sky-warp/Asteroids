using System;
using _Project.Scripts.GameOverServices;
using R3;
using Zenject;

namespace _Project.Scripts.UnityAds
{
    public class AdsController : IInitializable, IDisposable
    {
        private AdManager _adManager;
        private AdsView _adsView;
        private DefaultGameStateService _defaultGameStateService;
        private readonly CompositeDisposable _disposable = new();

        public AdsController(AdsView adsView, AdManager adManager, DefaultGameStateService defaultGameStateService)
        {
            _adManager = adManager;
            _adsView = adsView;
            _defaultGameStateService = defaultGameStateService;
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
            _adManager.RewardAd.WasWatched
                .Where(value => value == true)
                .Subscribe(_ => _defaultGameStateService.OnGameResume.OnNext(Unit.Default))
                .AddTo(_disposable);
        }

        public void Dispose()
        {
            _disposable?.Dispose();
        }
    }
}