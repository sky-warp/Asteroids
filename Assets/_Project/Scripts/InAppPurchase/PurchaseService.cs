using System;
using R3;
using UnityEngine;

namespace _Project.Scripts.InAppPurchase
{
    public class PurchaseService : IAppPurchaseBuyable, IDisposable
    {
        private IAPInitializer _initializer;
        private CompositeDisposable _disposable = new();

        public PurchaseService(IAPInitializer iAPInitializer)
        {
            _initializer = iAPInitializer;
        }

        public void PurchaseNoAds()
        {
            var product = _initializer.StoreController.products.WithID(_initializer.NoAdsProductData.ProductId);

            if (product.hasReceipt)
            {
                Debug.Log("You already remove ads");
            }
            else
            {
                _initializer.StoreController.InitiatePurchase(_initializer.NoAdsProductData.ProductId);
            }
        }

        public void PurchaseContinueGame()
        {
            var product = _initializer.StoreController.products.WithID(_initializer.ContinueGameProductData.ProductId);

            _initializer.StoreController.InitiatePurchase(_initializer.ContinueGameProductData.ProductId);
        }

        public void Dispose()
        {
            _disposable?.Dispose();
        }
    }
}