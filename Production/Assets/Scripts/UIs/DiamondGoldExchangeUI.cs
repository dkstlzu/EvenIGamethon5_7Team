using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MoonBunny.UIs
{
    public class DiamondGoldExchangeUI : UI
    {
        [SerializeField] private TextMeshProUGUI _currentGoldText;
        [SerializeField] private TextMeshProUGUI _currentDiamondText;
        [SerializeField] private Button _exchangeOnceButton;
        [SerializeField] private Button _exchangeTenTimeButton;
        
        [SerializeField] private TextMeshProUGUI _oncePriceText;
        [SerializeField] private TextMeshProUGUI _tenTimeBeforeSalePriceText;
        [SerializeField] private TextMeshProUGUI _tenTimePriceText;

        public int DiamondNumberPerTrade;
        public int GoldNumberPerTrade;
        public int GoldNumberPerTenTimeTrade;

        protected override void Rebuild()
        {
            _currentGoldText.text = GameManager.instance.GoldNumber.ToString();
            _currentDiamondText.text = GameManager.instance.DiamondNumber.ToString();

            _oncePriceText.text = GoldNumberPerTrade.ToString();
            _tenTimeBeforeSalePriceText.text = (GoldNumberPerTrade * 10).ToString();
            _tenTimePriceText.text = GoldNumberPerTenTimeTrade.ToString();
            
            _exchangeOnceButton.interactable = GameManager.instance.GoldNumber >= GoldNumberPerTrade;
            _exchangeTenTimeButton.interactable = GameManager.instance.GoldNumber >= GoldNumberPerTrade * 10;
        }

        public void OnExchangeOnceButtonClicked()
        {
            ExchangeGoldToDiamond(1);
        }

        public void OnExchangeTenTimeButtonClicked()
        {
            ExchangeGoldToDiamond(10);
        }

        private void ExchangeGoldToDiamond(int time)
        {
            GameManager.instance.GoldNumber -= GoldNumberPerTrade * time;
            QuestManager.instance.GetDiamond(DiamondNumberPerTrade * time, false);
            
            Rebuild();
        }
    }
}