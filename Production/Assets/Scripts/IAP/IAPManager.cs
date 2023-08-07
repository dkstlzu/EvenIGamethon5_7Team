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

        [HideInInspector] public List<ProductInfo> ProductInfoList = new List<ProductInfo>();

        public ProductInfo Diamond10ProductInfo;
        public ProductInfo Diamond25ProductInfo;
        public ProductInfo Diamond50ProductInfo;
        public ProductInfo Diamond100ProductInfo;
        public ProductInfo Diamond300ProductInfo;
        public ProductInfo Diamond500ProductInfo;
        public ProductInfo Diamond1000ProductInfo;
        
        public ProductInfo LimitedPackageProductInfo;
        public ProductInfo UnlimitedPackageProductInfo;
        
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
            catch (Exception)
            {
                Debug.LogError("UnityService initialize error {e}");
                throw;
            }
            
            SetStore();
        }

        void SetStore()
        {
            ConfigurationBuilder builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

            builder.AddProduct(Diamond10ProductInfo.Id, ProductType.Consumable);
            builder.AddProduct(Diamond25ProductInfo.Id, ProductType.Consumable);
            builder.AddProduct(Diamond50ProductInfo.Id, ProductType.Consumable);
            builder.AddProduct(Diamond100ProductInfo.Id, ProductType.Consumable);
            builder.AddProduct(Diamond300ProductInfo.Id, ProductType.Consumable);
            builder.AddProduct(Diamond500ProductInfo.Id, ProductType.Consumable);
            builder.AddProduct(Diamond1000ProductInfo.Id, ProductType.Consumable);
            
            builder.AddProduct(LimitedPackageProductInfo.Id, ProductType.Consumable);
            builder.AddProduct(UnlimitedPackageProductInfo.Id, ProductType.Consumable);
            
            builder.AddProduct(RemoveAdProductInfo.Id, ProductType.NonConsumable);
            
            ProductInfoList.Add(Diamond10ProductInfo);
            ProductInfoList.Add(Diamond25ProductInfo);
            ProductInfoList.Add(Diamond50ProductInfo);
            ProductInfoList.Add(Diamond100ProductInfo);
            ProductInfoList.Add(Diamond300ProductInfo);
            ProductInfoList.Add(Diamond500ProductInfo);
            ProductInfoList.Add(Diamond1000ProductInfo);
            
            ProductInfoList.Add(LimitedPackageProductInfo);
            ProductInfoList.Add(UnlimitedPackageProductInfo);
            
            ProductInfoList.Add(RemoveAdProductInfo);

            UnityPurchasing.Initialize(this, builder);
        }

        public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
        {
            MoonBunnyLog.print("On IAP Initialize");
            
            _controller = controller;

            foreach (var info in ProductInfoList)
            {
                info.StoreController = controller;
            }

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

        public event Action<string> OnPurchaseComplete;

        public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs purchaseEvent)
        {
            Product product = purchaseEvent.purchasedProduct;

            string id = product.definition.id;
            
            MoonBunnyLog.print($"Purchase Complete {id}");

            if (id == Diamond10ProductInfo.Id)
            {
                QuestManager.instance.GetDiamond(Diamond10ProductInfo.RewardValue, false);
            } else if (id == Diamond25ProductInfo.Id)
            {
                QuestManager.instance.GetDiamond(Diamond25ProductInfo.RewardValue, false);
            } else if (id == Diamond50ProductInfo.Id)
            {
                QuestManager.instance.GetDiamond(Diamond50ProductInfo.RewardValue, false);
            } else if (id == Diamond100ProductInfo.Id)
            {
                QuestManager.instance.GetDiamond(Diamond100ProductInfo.RewardValue, false);
            } else if (id == Diamond300ProductInfo.Id)
            {
                QuestManager.instance.GetDiamond(Diamond300ProductInfo.RewardValue, false);
            } else if (id == Diamond500ProductInfo.Id)
            {
                QuestManager.instance.GetDiamond(Diamond500ProductInfo.RewardValue, false);
            } else if (id == Diamond1000ProductInfo.Id)
            {
                QuestManager.instance.GetDiamond(Diamond1000ProductInfo.RewardValue, false);
            } else if (id == LimitedPackageProductInfo.Id)
            {
                QuestManager.instance.GetDiamond(LimitedPackageProductInfo.RewardValue, false);
            } else if (id == UnlimitedPackageProductInfo.Id)
            {
                QuestManager.instance.GetDiamond(UnlimitedPackageProductInfo.RewardValue, false);
            } else if (id == RemoveAdProductInfo.Id)
            {
                GameManager.instance.RemoveAd = true;
            }
 
            OnPurchaseComplete?.Invoke(GetProductInfoWithId(id).Name);
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

        public ProductInfo GetProductInfo(string productName)
        {
            return ProductInfoList.Find(info => info.Name == productName);
        }
        
        public ProductInfo GetProductInfoWithId(string productId)
        {
            return ProductInfoList.Find(info => info.Id == productId);
        }
    }
}