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
        public RectTransform BoardTransform;
        
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

        private Coroutine _noticeCoroutine;

        private void Start()
        {
            Stage = GameManager.instance.Stage;

            OnExit += UncheckBoost;
        }

        void UncheckBoost()
        {
            if (Boost.Checked) Boost.OnClicked();
            OnExit -= UncheckBoost;
        }

        protected override void Rebuild()
        {
            if (Stage.RevivedNumber >= MaxReviveNumber || GameManager.instance.GoldNumber < REVIVE_GOLD_COST)
            {
                SetColorOfChildren(ReviveRect, MoonBunnyColor.DisabledColor);
                ReviveButton.interactable = false;
            }

            float randomValue = Random.value;
            
            if (randomValue <= BonusBoostPotential)
            {
                BoostUI.SetActive(true);
                BoardTransform.anchorMax = new Vector2(0.5f, 0.3f);
                BoardTransform.anchorMin = new Vector2(0.5f, 0.3f);
                
                bool isManget = Random.value > 0.5f;
                Boost.BoostName = isManget ? MagnetBoostEffect.BoostName : StarCandyBoostEffect.BoostName;
                Boost.BoostOnText = isManget ? "자석 부스트!" : "별사탕은 어떤가?";
                Boost.Price = isManget ? Random.Range(20, 30) : Random.Range(10, 20);
                Boost.PriceText.text = Boost.Price.ToString();
                Boost.BoostImage.sprite = isManget ? PreloadedResources.instance.BoostSpriteList[0] : PreloadedResources.instance.BoostSpriteList[1];

                _noticeCoroutine = StartCoroutine(NoticeCoroutine());
            }
            else
            {
                BoostUI.SetActive(false);
                BoardTransform.anchorMax = new Vector2(0.5f, 0.5f);
                BoardTransform.anchorMin = new Vector2(0.5f, 0.5f);
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