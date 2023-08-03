using System;
using MoonBunny.Dev;
using UnityEngine.Purchasing;

namespace MoonBunny
{
    [Serializable]
    public class ProductInfo
    {
        public string Name;
        public string Id;
        public string Description;
        public float Price;
        
        public IStoreController StoreController { get; set; }

        public void InitiatePurchase()
        {
            if (StoreController == null)
            {
                MoonBunnyLog.print("Unity Purchasing system is not initialized yet");
                return;
            }
            else
            {
                MoonBunnyLog.print($"InitiatePurcase {Id}");
            }
            StoreController.InitiatePurchase(Id);
        }
    }
}