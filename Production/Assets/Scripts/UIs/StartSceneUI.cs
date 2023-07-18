using System;
using DG.Tweening;
using dkstlzu.Utility;
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

        [Header("Stage Buttons")]
        public Button Stage1Button;
        public Button Stage2Button;
        public Button Stage3Button;
        public Button Stage4Button;
        public Button Stage5Button;
        public Button StageChallengeButton;

        private GameManager _gameManager;

        [RuntimeInitializeOnLoadMethod]
        static void InitializeOnLoad()
        {
            _showCutScene = true;
        }

        private void Start()
        {
            _gameManager = GameManager.instance;

            Stage2Button.interactable = _gameManager.Stage1Clear;
            Stage3Button.interactable = _gameManager.Stage2Clear;
            Stage4Button.interactable = _gameManager.Stage3Clear;
            Stage5Button.interactable = _gameManager.Stage4Clear;
            StageChallengeButton.interactable = _gameManager.Stage5Clear;

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