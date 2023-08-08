using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using MoonBunny.Effects;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace MoonBunny.UIs
{
    public class StageFailUI : UI
    {
        [Header("Boost Bonus")]
        public GameObject BoostUI;
        public TextMeshProUGUI BoostNoticeText;
        public BoostUI Boost;
        public float BonusBoostPotential;
        public string[] NoticeTextList;
        public float NoticeInterval;
        
        [Header("Native")]
        public Stage Stage;
        public StageUI StageUI;

        public int MaxReviveNumber;
        public RectTransform ReviveRect;
        public Button ReviveButton;

        public const int REVIVE_GOLD_COST = 100;

        private void Start()
        {
            Stage = GameManager.instance.Stage;
        }

        protected override void Rebuild()
        {
            if (Stage.RevivedNumber >= MaxReviveNumber || GameManager.instance.GoldNumber < REVIVE_GOLD_COST)
            {
                SetColorOfChildren(ReviveRect, MoonBunnyColor.DisabledColor);
                ReviveButton.interactable = false;
            }

            if (Random.value <= BonusBoostPotential)
            {
                BoostUI.SetActive(true);
                bool isManget = Random.value > 0.5f;
                Boost.BoostName = isManget ? MagnetBoostEffect.BoostName : StarCandyBoostEffect.BoostName;
                Boost.BoostOnText = isManget ? "자석 부스트!" : "별사탕은 어떤가?";
                Boost.Price = Random.Range(20, 50);
                Boost.PriceText.text = Boost.Price.ToString();
                Boost.BoostImage.sprite = isManget ? PreloadedResources.instance.BoostSpriteList[0] : PreloadedResources.instance.BoostSpriteList[1];

                StartCoroutine(NoticeCoroutine());
            }
            else
            {
                BoostUI.SetActive(false);
                ((RectTransform)transform).anchorMin = new Vector2(0.5f, 0.5f);
                ((RectTransform)transform).anchorMax = new Vector2(0.5f, 0.5f);
            }
        }

        IEnumerator NoticeCoroutine()
        {
            int index = 0;

            while (true)
            {
                BoostNoticeText.DOText(NoticeTextList[index], 0.2f);
                index++;
                if (index >= NoticeTextList.Length)
                {
                    index = 0;
                }

                yield return new WaitForSeconds(NoticeInterval);
            }
        }
    }
}