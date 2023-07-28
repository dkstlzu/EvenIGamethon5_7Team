﻿using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MoonBunny.UIs
{
    public class BoostUI : UI
    {
        public static int S_ConsumingGold = 0;

        [RuntimeInitializeOnLoadMethod]
        static void Init()
        {
            S_ConsumingGold = 0;
        }
        
        public string BoostName;
        public int Price;
        public TextMeshProUGUI PriceText;

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
        public Image CheckerImage;
        public bool Checked;

        private void Awake()
        {
            PriceText.text = Price + " 골드";
        }

        public void OnClicked()
        {
            if (Checked)
            {
                NoticeText.DOText(BoostOffText, TweenDuration);
                NoticeText.DOColor(NormalColor, TweenDuration);
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
                CheckerImage.enabled = !CheckerImage.enabled;
                Checked = true;
                S_ConsumingGold += Price;
            }
        }
    }
}