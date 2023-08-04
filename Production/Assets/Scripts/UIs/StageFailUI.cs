using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MoonBunny.UIs
{
    public class StageFailUI : UI
    {
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
        }
    }
}