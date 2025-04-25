using _Project.Scripts.InAppPurchase.Products;
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
        
        public IAPInitializer(NoAdsProductData noAdsProductData)
        {
            NoAdsProductData = noAdsProductData;
        }
        
        public void Init()
        {
            if (PlayerPrefs.GetInt("NoAdsWasPurchased") == 1)
            {
                return;
            }
            
            SetupBuilder();
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
                Debug.Log("No ads from now");
                PlayerPrefs.SetInt("NoAdsWasPurchased", 1);
                
                NoAdsWasPaid.Value = true;
                
                return PurchaseProcessingResult.Complete;
            }

            return PurchaseProcessingResult.Pending;
        }

        private void SetupBuilder()
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