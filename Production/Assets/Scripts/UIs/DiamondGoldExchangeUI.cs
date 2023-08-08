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
        public int DiamondNumberPerTenTimeTrade;
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
            _exchangeTenTimeButton.interactable = GameManager.instance.GoldNumber >= GoldNumberPerTenTimeTrade;
        }

        public void OnExchangeOnceButtonClicked()
        {
            ExchangeGoldToDiamond(GoldNumberPerTrade, DiamondNumberPerTrade);
        }

        public void OnExchangeTenTimeButtonClicked()
        {
            ExchangeGoldToDiamond(GoldNumberPerTenTimeTrade, DiamondNumberPerTenTimeTrade);
        }

        private void ExchangeGoldToDiamond(int goldNumber, int diamondNumber)
        {
            GameManager.instance.GoldNumber -= goldNumber;
            QuestManager.instance.GetDiamond(diamondNumber, false);
            
            Rebuild();
        }
    }
}