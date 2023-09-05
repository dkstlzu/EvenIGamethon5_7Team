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

        private const string MAX_PURCHASED = "최대한도로 구매함";
        private string LimitedDescription;
        private string UnlimitedDescription;

        public int CleareStageNumber;

        public ConfirmUI ConfirmUI; 
            
        protected override void Awake()
        {
            base.Awake();
            
            PackageToggle.onValueChanged.AddListener(OnPackageToggle);
            DiamondChargeToggle.onValueChanged.AddListener(OnDiamondChargeToggle);

            LimitedDescription = LimitedPackageDescription.text;
            UnlimitedDescription = UnlimitedPackageDescription.text;
            
            IAPManager.instance.OnPurchaseComplete += (productName) =>
            {
                if (productName == "패키지" || productName == "한정 패키지")
                {
                    PackageUpdate();
                }
            };
            
            OnExit += ResetUI;
        }

        private void ResetUI()
        {
            PackageToggle.isOn = false;
            DiamondChargeToggle.isOn = false;
        }

        protected override void Rebuild()
        {
            ConfirmUI.Description.text = "점프해바니의 서버는 아직 구현중입니다.\n앱을 삭제할시에 결제하신 정보가 남지 않습니다!\n죄송합니다! 꼭 인지하고 진행해주세요";
            ConfirmUI.Open();

            PackageUpdate();
        }

        private void PackageUpdate()
        {
            LimitedPackageButton.interactable = !GameManager.SaveData.LimitedPackagePurchased;
            if (!LimitedPackageButton.interactable)
            {
                LimitedPackageDescription.text = MAX_PURCHASED;
            }
            else
            {
                LimitedPackageDescription.text = LimitedDescription;
            }

            CleareStageNumber = 0;

            CleareStageNumber += QuestManager.instance.GetQuest(QuestManager.UNLOCK_STAGE_ID).State == QuestState.IsFinished || QuestManager.instance.GetQuest(QuestManager.UNLOCK_STAGE_ID).State == QuestState.CanTakeReward ? 1 : 0;
            CleareStageNumber += QuestManager.instance.GetQuest(QuestManager.UNLOCK_STAGE_ID+1).State == QuestState.IsFinished || QuestManager.instance.GetQuest(QuestManager.UNLOCK_STAGE_ID+1).State == QuestState.CanTakeReward ? 1 : 0;
            CleareStageNumber += QuestManager.instance.GetQuest(QuestManager.UNLOCK_STAGE_ID+2).State == QuestState.IsFinished || QuestManager.instance.GetQuest(QuestManager.UNLOCK_STAGE_ID+2).State == QuestState.CanTakeReward ? 1 : 0;
            CleareStageNumber += QuestManager.instance.GetQuest(QuestManager.UNLOCK_STAGE_ID+3).State == QuestState.IsFinished || QuestManager.instance.GetQuest(QuestManager.UNLOCK_STAGE_ID+3).State == QuestState.CanTakeReward ? 1 : 0;

            UnlimitedPackageButton.interactable = CleareStageNumber > GameManager.SaveData.UnlimitedPackagePurchasedNumber;
            if (!UnlimitedPackageButton.interactable)
            {
                UnlimitedPackageDescription.text = MAX_PURCHASED;
            }
            else
            {
                UnlimitedPackageDescription.text = UnlimitedDescription;
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