using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MoonBunny.UIs
{
    public class BoostUI : UI
    {
        public static int S_ConsumingGold = 0;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void Init()
        {
            S_ConsumingGold = 0;
        }
        
        public string BoostName;
        public int Price;
        public TextMeshProUGUI PriceText;
        public Image BoostImage;

        [Space(10)]
        public TextMeshProUGUI NoticeText;
        public string LackOfGoldText;
        public Color WarningColor;

        [Space(10)]
        public string BoostOnText;
        public string BoostOffText;
        public Color NormalColor;

        [Space(10)]
        public float TweenDuration;

        public Image CheckerBackgroundImage;
        public Image CheckerImage;
        public bool Checked;

        protected override void Awake()
        {
            base.Awake();
            
            PriceText.text = Price.ToString();
        }

        public void OnClicked()
        {
            if (Checked)
            {
                NoticeText.DOText(BoostOffText, TweenDuration);
                NoticeText.DOColor(NormalColor, TweenDuration);
                CheckerBackgroundImage.enabled = false;
                CheckerImage.enabled = false;
                Checked = false;
                S_ConsumingGold -= Price;
                return;
            }
                    
            if (GameManager.instance.GoldNumber - S_ConsumingGold < Price)
            {
                NoticeText.DOText(LackOfGoldText, TweenDuration);
                NoticeText.DOColor(WarningColor, TweenDuration);
            }
            else
            {
                NoticeText.DOText(BoostOnText, TweenDuration);
                NoticeText.DOColor(NormalColor, TweenDuration);
                CheckerBackgroundImage.enabled = !CheckerBackgroundImage.enabled;
                CheckerImage.enabled = !CheckerImage.enabled;
                Checked = true;
                S_ConsumingGold += Price;
            }
        }
    }
}