using System;
using System.Collections.Generic;
using DG.Tweening;
using dkstlzu.Utility;
using MoonBunny.Dev;
using MoonBunny.Effects;
using Prefabs.UIs;
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
        public TextMeshProUGUI GoldText1;
        public TextMeshProUGUI GoldText2;

        [Serializable]
        public class FriendLibraryUI
        {
            public Button SelectButton;
            public TextMeshProUGUI NameText;
            public Sprite ProfileSprite;
        }
        
        [Header("Sub UIs")] 
        public FriendProfileUI FriendProfileUI;

        public List<FriendLibraryUI> FriendLibraryUIList;
        [FormerlySerializedAs("CurrentProfileImage")] public Image ProfileImage;


        [Header("Boost")] 
        public List<BoostUI> BoostItemList;

        [Serializable]
        public class StageButtonUI
        {
            public Button Button;
            public Image StarImage;
            public Image LockImage;
        }
        
        [Header("Stage Buttons")] 
        public List<StageButtonUI> StageButtonList;
        public Sprite LockStageSprite;

        private StageName _stageName;
        private int _selectingLevel;
        private int _subLevelIndex;

        [Serializable]
        public class SubLevelUI
        {
            public Image Image;
            public Image Text;
            public bool Enabled;
            public List<SubLevelSprites> SpriteList;
        }

        [Serializable]
        public class SubLevelSprites
        {
            public Sprite SubLevelSprite;
            public Sprite SubLevelTextSprite;
        }

        [Header("Sub Levels")]
        public RectTransform SubLevelContent;
        public float SubLevelSwipeTime;
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
            
            for (int i = 0; i < FriendLibraryUIList.Count; i++)
            {
                if (!FriendCollectionManager.instance.Collection.Datas[i].Finish()) continue;
                
                FriendLibraryUIList[i].SelectButton.interactable = true;
                string name = StringValue.GetStringValue(FriendCollectionManager.instance.Collection.Datas[i].Name);
                FriendLibraryUIList[i].NameText.text = name;
            }
        }

        public void OnSettingButtonClicked()
        {
            FadeIn(SettingUICanvasGroup);
        }

        public void OnSettingCloseButtonClicked()
        {
            FadeOut(SettingUICanvasGroup);
        }

        public void OnFriendButtonClicked(string friendName)
        {
            FriendName name;

            if (Enum.TryParse(friendName, out name))
            {
                FadeIn(FriendProfileUICanvasGroup);
                FriendProfileUI.Set(name);
                FriendProfileUI.Open();
            }
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
            FadeOut(FriendProfileUICanvasGroup);
        }

        public void OnBackToFriendSelectButtonClicked()
        {
            FadeOut(StageSelectCanvasGroup);                    
            FadeIn(FriendSelectCanvasGroup);                  
        }
        
        public void OnStageButtonClicked(string name)
        {
            if (!StageName.TryParse(name, true, out _stageName))
            {
                Debug.LogError("StageName is wrong while stage button clicked");
                return;
            }

            _selectingLevel = (int)_stageName;

            for (int i = 0; i < SubLevelList.Count; i++)
            {
                SubLevelList[i].Enabled = _gameManager.ClearDict[(StageName)_selectingLevel] >= i;
                SubLevelList[i].Image.sprite = SubLevelList[i].Enabled ? SubLevelList[i].SpriteList[_selectingLevel].SubLevelSprite : LockStageSprite;
                SubLevelList[i].Text.sprite = SubLevelList[i].SpriteList[_selectingLevel].SubLevelTextSprite;
            }

            FadeIn(SubLevelSelectCanvasGroup);
        }

        public void OnSubLevelSelecterToLeftButtonClicked()
        {
            _subLevelIndex--;
            _subLevelIndex = Mathf.Clamp(_subLevelIndex, 0, 2);
            SubLevelContent.DOPivot(new Vector2(_subLevelIndex / 2f, 0.5f), SubLevelSwipeTime);
            StartButton.interactable = SubLevelList[_subLevelIndex].Enabled;
        }

        public void OnSubLevelSelecterToRightButtonClicked()
        {
            _subLevelIndex++;
            _subLevelIndex = Mathf.Clamp(_subLevelIndex, 0, 2);
            SubLevelContent.DOPivot(new Vector2(_subLevelIndex / 2f, 0.5f), SubLevelSwipeTime);
            StartButton.interactable = SubLevelList[_subLevelIndex].Enabled;
        }
        
        public void OnSubLevelSelecterExitButtonClicked()
        {
            FadeOut(SubLevelSelectCanvasGroup);
        }

        public void OnStartButtonClicked()
        {
            SceneManager.LoadSceneAsync(StringValue.GetStringValue(_stageName, _subLevelIndex)).completed += (ao) =>
            {
                Stage stage = GameManager.instance.Stage;
                stage.SubLevel = _subLevelIndex;

                Character character = GameObject.FindWithTag("Player").GetComponent<Character>();

                foreach (BoostUI boost in BoostItemList)
                {
                    if (!boost.Checked) continue;
                    
                    switch (boost.BoostName)
                    {
                        case RocketBoostEffect.BoostName:
                            stage.BoostEffectList.Add(new RocketBoostEffect(character.Rigidbody));
                            break;
                        case MagnetBoostEffect.BoostName:
                            stage.BoostEffectList.Add(new MagnetBoostEffect(character));
                            break;
                        case StarCandyBoostEffect.BoostName:
                            stage.BoostEffectList.Add(new StarCandyBoostEffect());
                            break;
                    }
                }
                
                _gameManager.GoldNumber -= BoostUI.S_ConsumingGold;
                BoostUI.S_ConsumingGold = 0;
                _gameManager.SaveProgress();
            };
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