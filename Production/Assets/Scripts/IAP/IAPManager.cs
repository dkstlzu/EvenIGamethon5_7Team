using System;
using System.Collections.Generic;
using dkstlzu.Utility;
using MoonBunny.Dev;
using Unity.Services.Core;
using Unity.Services.Core.Environments;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Extension;
using UnityEngine.Serialization;

namespace MoonBunny
{
    public class IAPManager : Singleton<IAPManager>, IDetailedStoreListener
    {
        private IStoreController _controller;

        public ProductInfo Diamond100ProductInfo;
        public ProductInfo Diamond200ProductInfo;
        public ProductInfo Diamond1000ProductInfo;
        public ProductInfo RemoveAdProductInfo;

        private const string DEV_ENV_KEY = "development";
        private const string PRODUCT_ENV_KEY = "production";

        private void Start()
        {
            InitializationOptions options = new InitializationOptions().SetEnvironmentName(DEV_ENV_KEY);
            try
            {
                UnityServices.InitializeAsync(options).ContinueWith(task => MoonBunnyLog.print("Unity Service initialization success."));
            }
            catch (Exception e)
            {
                Debug.LogError("UnityService initialize error {e}");
                throw;
            }
            
            SetStore();
        }

        void SetStore()
        {
            ConfigurationBuilder builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

            builder.AddProduct(Diamond100ProductInfo.Id, ProductType.Consumable);
            builder.AddProduct(Diamond200ProductInfo.Id, ProductType.Consumable);
            builder.AddProduct(Diamond1000ProductInfo.Id, ProductType.Consumable);
            builder.AddProduct(RemoveAdProductInfo.Id, ProductType.NonConsumable);

            UnityPurchasing.Initialize(this, builder);
        }

        public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
        {
            MoonBunnyLog.print("On IAP Initialize");
            
            _controller = controller;

            Diamond100ProductInfo.StoreController = controller;
            Diamond200ProductInfo.StoreController = controller;
            Diamond1000ProductInfo.StoreController = controller;
            RemoveAdProductInfo.StoreController = controller;

            CheckRemoveAd(RemoveAdProductInfo.Id);
        }

        private void CheckRemoveAd(string id)
        {
            if (_controller == null) return;
            
            Product product = _controller.products.WithID(id);
            if (product == null) return;

            if (product.hasReceipt)
            {
                
            }
            else
            {
                
            }
        }

        public void OnInitializeFailed(InitializationFailureReason error)
        {
            Debug.LogError(error);
        }

        public void OnInitializeFailed(InitializationFailureReason error, string message)
        {
            Debug.LogError(error + message);
        }

        public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs purchaseEvent)
        {
            Product product = purchaseEvent.purchasedProduct;

            string id = product.definition.id;
            
            MoonBunnyLog.print($"Purchase Complete {id}");

            if (id == Diamond100ProductInfo.Id)
            {
                GameManager.instance.DiamondNumber += 100;
            } else if (id == Diamond200ProductInfo.Id)
            {
                GameManager.instance.DiamondNumber += 200;
            } else if (id == Diamond1000ProductInfo.Id)
            {
                GameManager.instance.DiamondNumber += 1000;
            } else if (id == RemoveAdProductInfo.Id)
            {
                GameManager.instance.RemoveAd = true;
            }
 
            return PurchaseProcessingResult.Complete;
        }

        public void OnPurchaseFailed(Product productInfo, PurchaseFailureReason failureReason)
        {
            Debug.LogError($"Fail to purchase {productInfo.definition.id} because of {failureReason}");
        }

        public void OnPurchaseFailed(Product productInfo, PurchaseFailureDescription failureDescription)
        {
            Debug.LogError($"Fail to purchase {productInfo.definition.id} because of {failureDescription}");
        }
    }
}