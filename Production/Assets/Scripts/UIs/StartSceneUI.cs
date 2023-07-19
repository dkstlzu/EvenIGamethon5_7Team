using System;
using DG.Tweening;
using dkstlzu.Utility;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
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

        private void Start()
        {
            _gameManager = GameManager.instance;

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

            _gameManager.StartSceneUI = this;
            DiamonText1.text = _gameManager.DiamondNumber.ToString();
            DiamonText2.text = _gameManager.DiamondNumber.ToString();
            GoldText1.text = _gameManager.GoldNumber.ToString();
            GoldText2.text = _gameManager.GoldNumber.ToString();

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

        public void OnStartButtonClicked()
        {
            
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
            StageName stageName;
            if (!StageName.TryParse(name+"1", true, out stageName))
            {
                Debug.LogError("StageName is wrong while stage button clicked");
                return;
            }

            SceneManager.LoadScene(StringValue.GetStringValue(stageName));
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