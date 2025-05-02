using System;
using _Project.Scripts.InAppPurchase.View;
using _Project.Scripts.SaveSystems;
using R3;
using Zenject;

namespace _Project.Scripts.InAppPurchase.Controller
{
    public class IAPController : IInitializable, IDisposable
    {
        public readonly ReactiveProperty<bool> IsNoAds = new();

        private IAPView _view;
        private SaveData _saveData;
        private IAppPurchaseBuyable _purchaseService;
        private CompositeDisposable _disposable = new();

        public IAPController(IAPView view, IAppPurchaseBuyable purchaseService, SaveData saveData)
        {
            _view = view;
            _purchaseService = purchaseService;
            _saveData = saveData;
        }

        public void Initialize()
        {
            _view.NoAdsButton.onClick.AsObservable()
                .Subscribe(_ => _purchaseService.PurchaseNoAds())
                .AddTo(_disposable);
            
            _view.ContinueGameButton.onClick.AsObservable()
                .Subscribe(_ => _purchaseService.PurchaseContinueGame())
                .AddTo(_disposable);

            _saveData.SetOnGameContinueStatus(true);
            
            IsNoAds
                .Where(_ => _saveData.NoAdsPurchaseStatus == 1)
                .Subscribe(_ => IsNoAds.Value = true)
                .AddTo(_disposable);
            IsNoAds
                .Subscribe(value => _view.NoAdsButton.interactable = !value)
                .AddTo(_disposable);
            IsNoAds
                .Subscribe(value => _view.ContinueGameButton.interactable = value)
                .AddTo(_disposable);
        }

        public void Dispose()
        {
            _disposable?.Dispose();
        }
    }
}