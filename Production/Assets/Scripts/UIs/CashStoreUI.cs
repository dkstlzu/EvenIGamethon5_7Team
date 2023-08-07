using UnityEngine;
using UnityEngine.UI;

namespace MoonBunny.UIs
{
    public class CashStoreUI : UI
    {
        [SerializeField] private Toggle PackageToggle;
        [SerializeField] private Toggle DiamondChargeToggle;

        [SerializeField] private Image PackageCheckerBackground;
        [SerializeField] private Image PackageCheckerImage;
        [SerializeField] private Image DiamondChargeCheckerBackground;
        [SerializeField] private Image DiamondChargeCheckerImage;

        [SerializeField] private GameObject PackageReceiptGO;
        [SerializeField] private GameObject DiamondChargeReceiptGO;

        protected override void Awake()
        {
            base.Awake();
            
            PackageToggle.onValueChanged.AddListener(OnPackageToggle);
            DiamondChargeToggle.onValueChanged.AddListener(OnDiamondChargeToggle);
        }

        public void OnPackageToggle(bool isOn)
        {
            if (isOn)
            {
                DiamondChargeToggle.isOn = false;
            }
            
            PackageCheckerBackground.enabled = isOn;
            PackageCheckerImage.enabled = isOn;
            
            PackageReceiptGO.SetActive(isOn);
        }

        public void OnDiamondChargeToggle(bool isOn)
        {
            if (isOn)
            {
                PackageToggle.isOn = false;
            }
            
            DiamondChargeCheckerBackground.enabled = isOn;
            DiamondChargeCheckerImage.enabled = isOn;
            
            DiamondChargeReceiptGO.SetActive(isOn);
        }

        public void OnLimitedPackagePurchase()
        {
            IAPManager.instance.LimitedPackageProductInfo.InitiatePurchase();
        }

        public void OnUnlimitedPackagePurchase()
        {
            IAPManager.instance.UnlimitedPackageProductInfo.InitiatePurchase();
        }

        public void OnChargeButtonClicked(string productName)
        {
            IAPManager.instance.GetProductInfo(productName).InitiatePurchase();
        }
    }
}