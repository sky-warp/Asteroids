using System;
using _Project.Scripts.GameOverServices;
using _Project.Scripts.SceneManagers;
using _Project.Scripts.UnityAds.View;
using R3;
using Zenject;

namespace _Project.Scripts.UnityAds.Controller
{
    public class AdsController : IInitializable, IDisposable
    {
        private IAdShowable _adManager;
        private AdsView _adsView;
        private DefaultGameStateService _defaultGameStateService;
        private readonly CompositeDisposable _disposable = new();
        private SceneManager _sceneManager;

        public AdsController(AdsView adsView, IAdShowable adManager, DefaultGameStateService defaultGameStateService,
            SceneManager sceneManager)
        {
            _adManager = adManager;
            _adsView = adsView;
            _defaultGameStateService = defaultGameStateService;
            _sceneManager = sceneManager;
        }

        public void Initialize()
        {
            _adsView.ShowRewardedAdButton.OnClickAsObservable()
                .Subscribe(_ => _adManager.ShowRewardAd())
                .AddTo(_disposable);

            _adManager.RewardAdWasWatched
                .Where(value => value == true)
                .Subscribe(_ => _adsView.ShowRewardedAdButton.interactable = false)
                .AddTo(_disposable);
            _adManager.RewardAdWasWatched
                .Where(value => value == true)
                .Subscribe(_ => _defaultGameStateService.OnGameResume.OnNext(Unit.Default))
                .AddTo(_disposable);

            _sceneManager.SceneChanged
                .Subscribe(_ => _adManager.RewardAd.WasWatched.Value = false)
                .AddTo(_disposable);
        }

        public void Dispose()
        {
            _disposable?.Dispose();
        }
    }
}