using _Project.Scripts.InAppPurchase.Products;
using _Project.Scripts.SaveSystems;
using R3;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Extension;

namespace _Project.Scripts.InAppPurchase
{
    public class IAPInitializer : IDetailedStoreListener
    {
        public readonly ReactiveProperty<bool> NoAdsWasPaid = new();

        public NoAdsProductData NoAdsProductData { get; private set; }
        public IStoreController StoreController { get; private set; }

        private NoAdsSaveData _noAdsSaveData;
        private NoAdsSaveSystem _noAdsSaveSystem;

        public IAPInitializer(NoAdsProductData noAdsProductData, NoAdsSaveData noAdsSaveData,
            NoAdsSaveSystem noAdsSaveSystem)
        {
            NoAdsProductData = noAdsProductData;
            _noAdsSaveData = noAdsSaveData;
            _noAdsSaveSystem = noAdsSaveSystem;
        }

        public void Init()
        {
            if (_noAdsSaveSystem.CheckBuyStatus())
            {
                NoAdsWasPaid.Value = true;
                return;
            }

            SetupNoAdsPurchase();
        }

        public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
        {
            StoreController = controller;
        }

        public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs purchaseEvent)
        {
            var product = purchaseEvent.purchasedProduct;

            if (product.definition.id == NoAdsProductData.ProductId)
            {
                _noAdsSaveData.SetPurchaseStatus(1);
                
                Debug.Log("No ads from now");

                NoAdsWasPaid.Value = true;

                return PurchaseProcessingResult.Complete;
            }

            return PurchaseProcessingResult.Pending;
        }

        private void SetupNoAdsPurchase()
        {
            var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

            builder.AddProduct(NoAdsProductData.ProductId, ProductType.NonConsumable);

            UnityPurchasing.Initialize(this, builder);
        }

        public void OnInitializeFailed(InitializationFailureReason error)
        {
            Debug.Log("Cannot initialize product");
        }

        public void OnInitializeFailed(InitializationFailureReason error, string message)
        {
            Debug.Log("Cannot initialize product");
        }

        public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
        {
            Debug.Log("Faile when purchasing product");
        }

        public void OnPurchaseFailed(Product product, PurchaseFailureDescription failureDescription)
        {
            Debug.Log("Faile when purchasing product");
        }
    }
}