using System;
using _Project.Scripts.InAppPurchase.View;
using R3;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.InAppPurchase.Controller
{
    public class IAPController : IInitializable, IDisposable
    {
        public readonly ReactiveProperty<bool> IsNoAds = new();

        private IAPView _view;
        private InAppPurchaseBuyable _purchaseService;
        private CompositeDisposable _disposable = new();

        public IAPController(IAPView view, InAppPurchaseBuyable purchaseService)
        {
            _view = view;
            _purchaseService = purchaseService;
        }

        public void Initialize()
        {
            if (PlayerPrefs.GetInt("NoAdsWasPurchased") == 1)
            {
                _view.NoAdsButton.interactable = false;
                IsNoAds.Value = true;
            }
            else
            {
                _view.NoAdsButton.interactable = true;
                
                _view.NoAdsButton.onClick.AsObservable()
                    .Subscribe(_ => _purchaseService.PurchaseNoAds())
                    .AddTo(_disposable);

                _purchaseService.IsNoAds
                    .Subscribe(value => this.IsNoAds.Value = value)
                    .AddTo(_disposable);

                IsNoAds
                    .Subscribe(value => _view.NoAdsButton.interactable = !value)
                    .AddTo(_disposable);
            }
        }

        public void Dispose()
        {
            _disposable?.Dispose();
        }
    }
}