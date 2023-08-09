using System;
using dkstlzu.Utility;
using UnityEngine;
using UnityEngine.UI;

namespace MoonBunny.UIs
{
    public class TutorialUI : UI
    {
        public Button DontShowAgainButton;
        public ConfirmUI ConfirmUI;
        public Animator CountDownAnimator;
        
        protected override void Awake()
        {
            base.Awake();

            OnOpen += () =>
            {
                TimeUpdatable.GlobalSpeed = 0;
                CountDownAnimator.enabled = false;
            };
            
            OnExit += () =>
            {
                TimeUpdatable.GlobalSpeed = 1;
                CountDownAnimator.enabled = true;
            };
        }
        
        public void OnDontShowTutorialButtonClicked()
        {
            ConfirmUI.Description.text = "튜토리얼을 다시 보고 싶으시면\n설정 창에서 변경할 수 있습니다.\n튜토리얼을 비활성화 하시겠습니까?";
            ConfirmUI.AddConfirmListener(() =>
            {
                GameManager.instance.ShowTutorial = false;
                DontShowAgainButton.interactable = false;
            });
            ConfirmUI.Open();
        }
    }
}