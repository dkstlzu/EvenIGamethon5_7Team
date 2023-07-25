using System;
using System.Collections.Generic;
using DG.Tweening;
using dkstlzu.Utility;
using MoonBunny.Dev;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace MoonBunny.UIs
{
    public class StartSceneUI : MonoBehaviour
    {
        public DOTweenAnimation ClickToStartDoTweenAnimation;
        private readonly float _fadeDuration = 1;

        private static bool _showCutScene = true;

        [Header("Main UI Flow")]
        public CanvasGroup IntroCanvasGroup;
        public CanvasGroup MainIntroCanvasGroup;
        public CanvasGroup FriendSelectCanvasGroup;
        public CanvasGroup StageSelectCanvasGroup;
        public CanvasGroup SubLevelSelectCanvasGroup;

        [Header("Sub Pop Ups")] 
        public CanvasGroup SettingUICanvasGroup;
        public CanvasGroup FriendProfileUICanvasGroup;
        public CanvasGroup StoryBookUICanvasGroup;
        public CanvasGroup InventoryUICanvasGroup;
        public CanvasGroup StoreUICanvasGroup;

        [Header("Money Texts")] 
        public TextMeshProUGUI DiamonText1;
        public TextMeshProUGUI DiamonText2;
        public TextMeshProUGUI GoldText1;
        public TextMeshProUGUI GoldText2;

        [Serializable]
        public class StageButtonUI
        {
            public Button Button;
            public Image StarImage;
            public Image LockImage;
        }
        
        [Header("Stage Buttons")] 
        public List<StageButtonUI> StageButtonList;
        
        private StageName _stageName;
        private int _selectingLevel;
        private int _subLevelIndex;

        [Serializable]
        public class SubLevelUI
        {
            public Image Image;
            public TextMeshProUGUI Text;
            public bool Enabled;
        }
        
        public RectTransform SubLevelContent;
        public float SubLevelSwipeTime;
        public Sprite LockStageSprite;
        public List<SubLevelUI> SubLevelList;
        
        public Button StartButton;

        [Header("Star Sprites")] public List<Sprite> StarSpriteList;

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
            
            DiamonText1.text = _gameManager.DiamondNumber.ToString();
            DiamonText2.text = _gameManager.DiamondNumber.ToString();
            GoldText1.text = _gameManager.GoldNumber.ToString();
            GoldText2.text = _gameManager.GoldNumber.ToString();

            for (int i = 0; i < StageButtonList.Count; i++)
            {
                int subLevelClear = _gameManager.ClearDict[(StageName)i];
                StageButtonList[i].StarImage.sprite = StarSpriteList[subLevelClear];
                
                if (i+1 < StageButtonList.Count)
                {
                    StageButtonList[i+1].Button.interactable = subLevelClear > 2;
                    StageButtonList[i+1].LockImage.enabled = subLevelClear <= 2;
                }
            }
        }

        private void Start()
        {
            if (!_showCutScene)
            {
                SkipIntro();
            }

            _showCutScene = false;
        }

        public void OnSettingButtonClicked()
        {
            FadeIn(SettingUICanvasGroup);
        }

        public void OnSettingCloseButtonClicked()
        {
            FadeOut(SettingUICanvasGroup);
        }

        public void OnFriendButtonClicked()
        {
            FadeIn(FriendProfileUICanvasGroup);
        }

        public void OnFriendProfileCloseClicked()
        {
            FadeOut(FriendProfileUICanvasGroup);
        }

        public void OnStoryBookButtonClicked()
        {
            FadeIn(StoryBookUICanvasGroup);
        }

        public void OnStoryBookCloseClicked()
        {
            FadeOut(StoryBookUICanvasGroup);
        }

        public void OnInventoryButtonClicked()
        {
            FadeIn(InventoryUICanvasGroup);
        }

        public void OnInventoryCloseButtonClicked()
        {
            FadeOut(InventoryUICanvasGroup);
        }

        public void OnStoreButtonClicked()
        {
            FadeIn(StoreUICanvasGroup);
        }

        public void OnStoreCloseButtonClicked()
        {
            FadeOut(StoreUICanvasGroup);
        }

        public void OnGoToStageSelectButtonClicked()
        {
            FadeIn(StageSelectCanvasGroup);
            FadeOut(FriendSelectCanvasGroup);
        }

        public void OnBackToFriendSelectButtonClicked()
        {
            FadeOut(StageSelectCanvasGroup);                    
            FadeIn(FriendSelectCanvasGroup);                  
        }

        public void OnSubLevelSelecterToLeftButtonClicked()
        {
            _subLevelIndex--;
            _subLevelIndex = Mathf.Clamp(_subLevelIndex, 0, 2);
            SubLevelContent.DOAnchorPos(new Vector2(_subLevelIndex * -250, 0), SubLevelSwipeTime, true);
            StartButton.interactable = SubLevelList[_subLevelIndex].Enabled;
        }

        public void OnSubLevelSelecterToRightButtonClicked()
        {
            _subLevelIndex++;
            _subLevelIndex = Mathf.Clamp(_subLevelIndex, 0, 2);
            SubLevelContent.DOAnchorPos(new Vector2(_subLevelIndex * -250, 0), SubLevelSwipeTime, true);
            StartButton.interactable = SubLevelList[_subLevelIndex].Enabled;
        }
        
        public void OnSubLevelSelecterExitButtonClicked()
        {
            FadeOut(SubLevelSelectCanvasGroup);
        }

        public void OnStartButtonClicked()
        {
            _stageName = _stageName + _subLevelIndex - 1; 
            SceneManager.LoadScene(StringValue.GetStringValue(_stageName));
        }

        public void OnDiamondPlusButtonClicked()
        {
            
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
                FadeIn(FriendSelectCanvasGroup);
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
            FriendSelectCanvasGroup.alpha = 1;
            FriendSelectCanvasGroup.blocksRaycasts = true;
        }

        public void OnExitButtonClicked()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

        public void OnStageButtonClicked(string name)
        {
            if (!StageName.TryParse(name, true, out _stageName))
            {
                Debug.LogError("StageName is wrong while stage button clicked");
                return;
            }

            _selectingLevel = (int)_stageName;
            int selectingLevelText = IntValue.GetEnumIntValue(_stageName);

            Sprite sprite = StageButtonList[_selectingLevel].Button.image.sprite;
            
            SubLevelList[0].Enabled = true;
            SubLevelList[1].Enabled = _gameManager.ClearDict[(StageName)_selectingLevel] >= 1;
            SubLevelList[2].Enabled = _gameManager.ClearDict[(StageName)_selectingLevel] >= 2;

            SubLevelList[0].Image.sprite = sprite;
            SubLevelList[1].Image.sprite = SubLevelList[1].Enabled ? sprite : LockStageSprite;
            SubLevelList[2].Image.sprite = SubLevelList[2].Enabled ? sprite : LockStageSprite;
            
            SubLevelList[0].Text.text = "스테이지 " + selectingLevelText + "-1";
            SubLevelList[1].Text.text = "스테이지 " + selectingLevelText + "-2";
            SubLevelList[2].Text.text = "스테이지 " + selectingLevelText + "-3";

            FadeIn(SubLevelSelectCanvasGroup);
        }

        private void FadeIn(CanvasGroup cg)
        {
            cg.DOFade(1, _fadeDuration);
            cg.blocksRaycasts = true;
        }

        private void FadeOut(CanvasGroup cg)
        {
            cg.DOFade(0, _fadeDuration);
            cg.blocksRaycasts = false;
        }
    }
}