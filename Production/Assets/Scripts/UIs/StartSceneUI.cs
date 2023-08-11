using System;
using System.Collections.Generic;
using DG.Tweening;
using dkstlzu.Utility;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace MoonBunny.UIs
{
    public class StartSceneUI : UI
    {
        [Header("Main UI Flow")]
        public FriendSelectUI FriendSelectUI;
        public StageSelectUI StageSelectUI;

        [Header("Sub Pop Ups")] 
        public SettingUI SettingUI;
        public FriendProfileUI FriendProfileUI;
        public QuestUI QuestUI;
        public StoreUI StoreUI;
        public DiamondGoldExchangeUI ExchangeUI;
        public ConfirmUI ConfirmUI;
        [Multiline(5)]
        public string ReallyQuitText;

        [Header("Money Texts")] 
        public TextMeshProUGUI GoldText1;
        public TextMeshProUGUI GoldText2;
        public TextMeshProUGUI DiamondText1;
        public TextMeshProUGUI DiamondText2;
        public Image ProfileImage1;
        public Image ProfileImage2;

        private GameManager _gameManager;

        protected override void Awake()
        {
            _gameManager = GameManager.instance;
            _gameManager.StartSceneUI = this;
            
            ProfileImage1.sprite = FriendSelectUI.FriendLibraryUIList[(int)_gameManager.UsingFriendName].ProfileSprite;
            ProfileImage2.sprite = FriendSelectUI.FriendLibraryUIList[(int)_gameManager.UsingFriendName].ProfileSprite;

            GoldText1.text = _gameManager.GoldNumber.ToString();
            GoldText2.text = _gameManager.GoldNumber.ToString();
            DiamondText1.text = _gameManager.DiamondNumber.ToString();
            DiamondText2.text = _gameManager.DiamondNumber.ToString();
            
            InputManager.instance.InputAsset.UI.Cancel.performed += OnQuitButtonClicked;
        }

        private void OnDestroy()
        {
            InputManager.instance.InputAsset.UI.Cancel.performed -= OnQuitButtonClicked;
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

        public void ShowEnding()
        {
            SceneManager.LoadScene(SceneName.Ending);
        }

        private void OnQuitButtonClicked(InputAction.CallbackContext context)
        {
            if (!ConfirmUI.isOpened)
            {
                OnQuitButtonClicked();
            }
        }

        public void OnQuitButtonClicked()
        {
            ConfirmUI.Description.text = ReallyQuitText;
            ConfirmUI.AddConfirmListener(() =>
            {
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#else
                Application.Quit();
#endif
            });
            ConfirmUI.Open();

        }

    }
}