using TMPro;
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

        [SerializeField] private Button LimitedPackageButton;
        [SerializeField] private Button UnlimitedPackageButton;

        [SerializeField] private TextMeshProUGUI LimitedPackageDescription;
        [SerializeField] private TextMeshProUGUI UnlimitedPackageDescription;

        protected override void Awake()
        {
            base.Awake();
            
            PackageToggle.onValueChanged.AddListener(OnPackageToggle);
            DiamondChargeToggle.onValueChanged.AddListener(OnDiamondChargeToggle);

            IAPManager.instance.OnPurchaseComplete += (productName) =>
            {
                if (productName == "패키지" || productName == "한정 패키지")
                {
                    Rebuild();
                }
            };
        }

        protected override void Rebuild()
        {
            base.Rebuild();

            LimitedPackageButton.interactable = !GameManager.ProgressSaveData.LimitedPackagePurchased;
            if (!LimitedPackageButton.interactable)
            {
                LimitedPackageDescription.text = "최대한도로 구매함";
            }

            int clearedStageNumber = 0;

            clearedStageNumber += QuestManager.instance.GetQuest(QuestManager.UNLOCK_STAGE_ID).State == QuestState.IsFinished || QuestManager.instance.GetQuest(QuestManager.UNLOCK_STAGE_ID).State == QuestState.CanTakeReward ? 1 : 0;
            clearedStageNumber += QuestManager.instance.GetQuest(QuestManager.UNLOCK_STAGE_ID+1).State == QuestState.IsFinished || QuestManager.instance.GetQuest(QuestManager.UNLOCK_STAGE_ID+1).State == QuestState.CanTakeReward ? 1 : 0;
            clearedStageNumber += QuestManager.instance.GetQuest(QuestManager.UNLOCK_STAGE_ID+2).State == QuestState.IsFinished || QuestManager.instance.GetQuest(QuestManager.UNLOCK_STAGE_ID+2).State == QuestState.CanTakeReward ? 1 : 0;
            clearedStageNumber += QuestManager.instance.GetQuest(QuestManager.UNLOCK_STAGE_ID+3).State == QuestState.IsFinished || QuestManager.instance.GetQuest(QuestManager.UNLOCK_STAGE_ID+3).State == QuestState.CanTakeReward ? 1 : 0;

            UnlimitedPackageButton.interactable = clearedStageNumber > GameManager.ProgressSaveData.UnlimitedPackagePurchasedNumber;
            if (!UnlimitedPackageButton.interactable)
            {
                UnlimitedPackageDescription.text = "최대한도로 구매함";
            }
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