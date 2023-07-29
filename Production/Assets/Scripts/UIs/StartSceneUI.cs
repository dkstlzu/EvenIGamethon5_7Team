using System;
using System.Collections.Generic;
using DG.Tweening;
using dkstlzu.Utility;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace MoonBunny.UIs
{
    public class StartSceneUI : UI
    {
        public DOTweenAnimation ClickToStartDoTweenAnimation;

        private static bool _showCutScene = true;

        [Header("Main UI Flow")]
        public CanvasGroup IntroCanvasGroup;
        public CanvasGroup MainIntroCanvasGroup;
        public FriendSelectUI FriendSelectUI;
        public StageSelectUI StageSelectUI;

        [Header("Sub Pop Ups")] 
        public SettingUI SettingUI;
        public FriendProfileUI FriendProfileUI;
        public QuestUI QuestUI;
        public StoreUI StoreUI;

        [Header("Money Texts")] 
        public TextMeshProUGUI GoldText1;
        public TextMeshProUGUI GoldText2;
        public Image ProfileImage1;
        public Image ProfileImage2;

        private GameManager _gameManager;

        [RuntimeInitializeOnLoadMethod]
        static void InitializeOnLoad()
        {
            _showCutScene = true;
        }

        private void Awake()
        {
            _gameManager = GameManager.instance;
            _gameManager.StartSceneUI = this;
            
            ProfileImage1.sprite = FriendSelectUI.FriendLibraryUIList[(int)_gameManager.UsingFriendName].ProfileSprite;
            ProfileImage2.sprite = FriendSelectUI.FriendLibraryUIList[(int)_gameManager.UsingFriendName].ProfileSprite;

            GoldText1.text = _gameManager.GoldNumber.ToString();
            GoldText2.text = _gameManager.GoldNumber.ToString();
        }

        private void Start()
        {
            if (!_showCutScene)
            {
                SkipIntro();
            }

            _showCutScene = false;
        }

        public void OnGoToStageSelectButtonClicked()
        {
            StageSelectUI.Open();
            FriendSelectUI.OnExitButtonClicked();
            FriendProfileUI.OnExitButtonClicked();
        }

        public void OnBackToFriendSelectButtonClicked()
        {
            StageSelectUI.OnExitButtonClicked();
            FriendSelectUI.Open();
        }


        public void OnGoldPlusButtonClicked()
        {
            
        }
        
        public void OnPressTheAnyKeyIntro(InputAction.CallbackContext callbackContext)
        {
            FadeOut(IntroCanvasGroup);
            MainIntroCanvasGroup.DOFade(1, 2);

            CoroutineHelper.Delay(() =>
            {
                FadeOut(MainIntroCanvasGroup);
                FriendSelectUI.Open();
            }, 5f);

            
            _gameManager.GetComponent<InputManager>().InputAsset.UI.Click.performed -= OnPressTheAnyKeyIntro;
        }

        public void AfterIntroAnimationFinish()
        {
            ClickToStartDoTweenAnimation.DOPlay();
            GetComponent<Animator>().enabled = false;
            _gameManager.GetComponent<InputManager>().InputAsset.UI.Click.performed += OnPressTheAnyKeyIntro;
        }

        public void SkipIntro()
        {
            GetComponent<Animator>().enabled = false;
            IntroCanvasGroup.alpha = 0;
            IntroCanvasGroup.blocksRaycasts = false;
            FriendSelectUI.Open(0);
        }

        public void OnExitButtonClicked()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

    }
}