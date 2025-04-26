using System;
using _Project.Scripts.InAppPurchase.View;
using R3;
using Zenject;

namespace _Project.Scripts.InAppPurchase.Controller
{
    public class IAPController : IInitializable, IDisposable
    {
        public readonly ReactiveProperty<bool> IsNoAds = new();
        public readonly ReactiveProperty<bool> IsContinue = new();

        private IAPView _view;
        private IAppPurchaseBuyable _purchaseService;
        private CompositeDisposable _disposable = new();

        public IAPController(IAPView view, IAppPurchaseBuyable purchaseService)
        {
            _view = view;
            _purchaseService = purchaseService;
        }

        public void Initialize()
        {
            _view.NoAdsButton.onClick.AsObservable()
                .Subscribe(_ => _purchaseService.PurchaseNoAds())
                .AddTo(_disposable);
            
            _view.ContinueGameButton.onClick.AsObservable()
                .Subscribe(_ => _purchaseService.PurchaseContinueGame())
                .AddTo(_disposable);

            _purchaseService.IsNoAds
                .Subscribe(value => this.IsNoAds.Value = value)
                .AddTo(_disposable);
            _purchaseService.IsContinue
                .Subscribe(value => this.IsContinue.Value = value)
                .AddTo(_disposable);

            IsNoAds
                .Subscribe(value => _view.NoAdsButton.interactable = !value)
                .AddTo(_disposable);

            if (IsNoAds.Value)
            {
                _view.ContinueGameButton.interactable = IsNoAds.Value;
            }
            else if (!IsNoAds.Value)
            {
                _view.ContinueGameButton.interactable = IsNoAds.Value;
            }
        }

        public void Dispose()
        {
            _disposable?.Dispose();
        }
    }
}