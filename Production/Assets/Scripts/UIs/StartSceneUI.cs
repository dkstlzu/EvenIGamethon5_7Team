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

        [Header("Stage Buttons")]
        public Button Stage1Button;
        public Image Stage1StarImage;
        public Button Stage2Button;
        public Image Stage2LockImage;
        public Image Stage2StarImage;
        public Button Stage3Button;
        public Image Stage3LockImage;
        public Image Stage3StarImage;
        public Button Stage4Button;
        public Image Stage4LockImage;
        public Image Stage4StarImage;
        public Button Stage5Button;
        public Image Stage5LockImage;
        public Image Stage5StarImage;
        public Button StageChallengeButton;
        public Image StageChallengeLockImage;

        private StageName _stageName;
        private int _selectingLevel;
        private int _subLevelIndex = 1;
        public RectTransform SubLevelContent;
        public Image SubLevel1Image;
        public Image SubLevel2Image;
        public Image SubLevel3Image;
        public TextMeshProUGUI SubLevel1Text;
        public TextMeshProUGUI SubLevel2Text;
        public TextMeshProUGUI SubLevel3Text;
        private bool _subLevel1Enabled;
        private bool _subLevel2Enabled;
        private bool _subLevel3Enabled;
        public Button StartButton;
        
        [Header("Star Sprites")]
        public Sprite StarNoneSprite;
        public Sprite StarOneSprite;
        public Sprite StarTwoSprite;
        public Sprite StarThreeSprite;

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
            
            Stage1StarImage.sprite = _gameManager.Stage1_1Clear
                ? _gameManager.Stage1_2Clear ? _gameManager.Stage1_3Clear ? StarThreeSprite : StarTwoSprite : StarOneSprite
                : StarNoneSprite;
            Stage2Button.interactable = _gameManager.Stage1_3Clear;
            Stage2LockImage.enabled = !_gameManager.Stage1_3Clear;
            Stage2StarImage.sprite = _gameManager.Stage2_1Clear
                ? _gameManager.Stage2_2Clear ? _gameManager.Stage2_3Clear ? StarThreeSprite : StarTwoSprite : StarOneSprite
                : StarNoneSprite;
            Stage3Button.interactable = _gameManager.Stage2_3Clear;
            Stage3LockImage.enabled = !_gameManager.Stage2_3Clear;
            Stage3StarImage.sprite = _gameManager.Stage3_1Clear
                ? _gameManager.Stage3_2Clear ? _gameManager.Stage3_3Clear ? StarThreeSprite : StarTwoSprite : StarOneSprite
                : StarNoneSprite;
            Stage4Button.interactable = _gameManager.Stage3_3Clear;
            Stage4LockImage.enabled = !_gameManager.Stage3_3Clear;
            Stage4StarImage.sprite = _gameManager.Stage4_1Clear
                ? _gameManager.Stage4_2Clear ? _gameManager.Stage4_3Clear ? StarThreeSprite : StarTwoSprite : StarOneSprite
                : StarNoneSprite;
            Stage5Button.interactable = _gameManager.Stage4_3Clear;
            Stage5LockImage.enabled = !_gameManager.Stage4_3Clear;
            Stage5StarImage.sprite = _gameManager.Stage5_1Clear
                ? _gameManager.Stage5_2Clear ? _gameManager.Stage5_3Clear ? StarThreeSprite : StarTwoSprite : StarOneSprite
                : StarNoneSprite;
            StageChallengeButton.interactable = _gameManager.Stage5_3Clear;
            StageChallengeLockImage.enabled = !_gameManager.Stage5_3Clear;
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
            _subLevelIndex = Mathf.Clamp(_subLevelIndex, 1, 3);
            SubLevelContent.DOAnchorPos(new Vector2((_subLevelIndex - 1) * -250, 0), 1, true);
            StartButton.interactable = _subLevelIndex == 1 ? _subLevel1Enabled :
                _subLevelIndex == 2 ? _subLevel2Enabled :
                _subLevelIndex == 3 && _subLevel3Enabled;
        }

        public void OnSubLevelSelecterToRightButtonClicked()
        {
            _subLevelIndex++;
            _subLevelIndex = Mathf.Clamp(_subLevelIndex, 1, 3);
            SubLevelContent.DOAnchorPos(new Vector2((_subLevelIndex - 1) * -250, 0), 1, true);
            StartButton.interactable = _subLevelIndex == 1 ? _subLevel1Enabled :
                _subLevelIndex == 2 ? _subLevel2Enabled :
                _subLevelIndex == 3 && _subLevel3Enabled;
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
            if (!StageName.TryParse(name+"1", true, out _stageName))
            {
                Debug.LogError("StageName is wrong while stage button clicked");
                return;
            }

            _selectingLevel = IntValue.GetEnumIntValue(_stageName);

            if (_selectingLevel == 1)
            {
                Sprite sprite = Stage1Button.image.sprite;
                
                SubLevel1Image.sprite = sprite;
                SubLevel2Image.sprite = _gameManager.Stage1_1Clear ? sprite : Stage2LockImage.sprite;
                SubLevel3Image.sprite = _gameManager.Stage1_2Clear ? sprite : Stage2LockImage.sprite;

 ;           } else if (_selectingLevel == 2)
            {
                Sprite sprite = Stage2Button.image.sprite;

                SubLevel1Image.sprite = sprite;
                SubLevel2Image.sprite = _gameManager.Stage2_1Clear ? sprite : Stage2LockImage.sprite;
                SubLevel3Image.sprite = _gameManager.Stage2_2Clear ? sprite : Stage2LockImage.sprite;
            } else if (_selectingLevel == 3)
            {
                Sprite sprite = Stage3Button.image.sprite;

                SubLevel1Image.sprite = sprite;
                SubLevel2Image.sprite = _gameManager.Stage3_1Clear ? sprite : Stage2LockImage.sprite;
                SubLevel3Image.sprite = _gameManager.Stage3_2Clear ? sprite : Stage2LockImage.sprite;
            } else if (_selectingLevel == 4)
            {
                Sprite sprite = Stage4Button.image.sprite;

                SubLevel1Image.sprite = sprite;
                SubLevel2Image.sprite = _gameManager.Stage4_1Clear ? sprite : Stage2LockImage.sprite;
                SubLevel3Image.sprite = _gameManager.Stage4_2Clear ? sprite : Stage2LockImage.sprite;
            } else if (_selectingLevel == 5)
            {
                Sprite sprite = Stage5Button.image.sprite;

                SubLevel1Image.sprite = sprite;
                SubLevel2Image.sprite = _gameManager.Stage5_1Clear ? sprite : Stage2LockImage.sprite;
                SubLevel3Image.sprite = _gameManager.Stage5_2Clear ? sprite : Stage2LockImage.sprite;
            }
            
            SubLevel1Text.text = "스테이지 " + _selectingLevel + "-1";
            SubLevel2Text.text = "스테이지 " + _selectingLevel + "-2";
            SubLevel3Text.text = "스테이지 " + _selectingLevel + "-3";

            _subLevel1Enabled = true;
            _subLevel2Enabled = SubLevel2Image.sprite != Stage2LockImage.sprite;
            _subLevel3Enabled = SubLevel3Image.sprite != Stage2LockImage.sprite;
            
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