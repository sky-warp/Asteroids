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
        private IAPInitializer _initializer;
        private CompositeDisposable _disposable = new();
        
        public IAPController(IAPView view, IAPInitializer iAPInitializer)
        {
            _view = view;
            _initializer = iAPInitializer;
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
                _view.NoAdsButton.onClick.AsObservable()
                    .Subscribe(_ => PurchaseNoAds())
                    .AddTo(_disposable);
                
                _initializer.NoAdsWasPaid
                    .Where(value => value)
                    .Subscribe(value => IsNoAds.Value = true)
                    .AddTo(_disposable);
            }
        }

        private void PurchaseNoAds()
        {
            var product = _initializer.StoreController.products.WithID(_initializer.NoAdsProductData.ProductId);

            if (product.hasReceipt)
            {
                Debug.Log("You already remove ads");
            }
            else
            {
                _initializer.StoreController.InitiatePurchase(_initializer.NoAdsProductData.ProductId);
                _view.NoAdsButton.interactable = false;
            }
        }

        public void Dispose()
        {
            _disposable?.Dispose();
        }
    }
}