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
        
        private void Start()
        {
            Stage = GameManager.instance.Stage;
            
            OnOpen += () =>
            {
                if (Stage.RevivedNumber >= MaxReviveNumber)
                {
                    SetColorOfChildren(ReviveRect, MoonBunnyColor.DisabledColor);
                    ReviveButton.interactable = false;
                }
            };
        }
    }
}