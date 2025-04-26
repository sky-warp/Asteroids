using _Project.Scripts.GameStateServices;
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
        public readonly ReactiveProperty<bool> IsContinueWasPaid = new();

        public NoAdsProductData NoAdsProductData { get; private set; }
        public ContinueGameProductData ContinueGameProductData { get; private set; }
        public IStoreController StoreController { get; private set; }

        private NoAdsSaveData _noAdsSaveData;
        private NoAdsSaveSystem _noAdsSaveSystem;
        private DefaultGameStateService _defaultGameStateService;

        public IAPInitializer(NoAdsProductData noAdsProductData, NoAdsSaveData noAdsSaveData,
            NoAdsSaveSystem noAdsSaveSystem, ContinueGameProductData continueGameProductData,
            DefaultGameStateService defaultGameStateService)
        {
            NoAdsProductData = noAdsProductData;
            _noAdsSaveData = noAdsSaveData;
            _noAdsSaveSystem = noAdsSaveSystem;
            ContinueGameProductData = continueGameProductData;
            _defaultGameStateService = defaultGameStateService;
        }

        public void Init()
        {
            SetupPurchases();
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
            else if (product.definition.id == ContinueGameProductData.ProductId)
            {
                if (!IsContinueWasPaid.Value)
                {
                    IsContinueWasPaid.Value = true;
                    _defaultGameStateService.OnGameResume.OnNext(Unit.Default);
                }
            }

            return PurchaseProcessingResult.Pending;
        }

        private void SetupPurchases()
        {
            var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

            if (_noAdsSaveSystem.CheckBuyStatus())
            {
                NoAdsWasPaid.Value = true;
            }
            else
            {
                builder.AddProduct(NoAdsProductData.ProductId, ProductType.NonConsumable);
            }

            builder.AddProduct(ContinueGameProductData.ProductId, ProductType.Consumable);

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