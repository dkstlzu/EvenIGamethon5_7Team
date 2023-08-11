using System;
using System.Collections.Generic;
using System.Net.Sockets;
using DG.Tweening;
using dkstlzu.Utility;
using MoonBunny.Dev;
using MoonBunny.Effects;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace MoonBunny.UIs
{
    public class StageUI : UI
    {
        public CanvasGroup PauseUI;
        public StageFailUI FailUI;
        public CanvasGroup ClearUI;

        [SerializeField] private AudioClip _clearAudioClip;
        [SerializeField] private AudioClip _failAudioClip;

        public Stage Stage;
        [SerializeField] private Button _changeDirectionButton;

        [Header("Pause")] public Toggle SoundToggle;
        public TextMeshProUGUI StageInfoText;
        public TutorialUI TutorialUI;
        
        [Header("Progress")]
        [SerializeField] private Slider _progressBar;
        [SerializeField] private TextMeshProUGUI _scoreText;
        [SerializeField] private Slider _progressHeightBar;
        [SerializeField] private Image _progressHeightImage;
        public List<Sprite> CharacterProfileSpriteList;

        [SerializeField] private Sprite _progressEmptyStar;
        [SerializeField] private Sprite _progressFullStar;
        [SerializeField] private Image _firstStar;
        [SerializeField] private Image _secondStar;
        [SerializeField] private Image _thirdStar;
        [SerializeField] private Image _firstChecker;
        [SerializeField] private Image _secondChecker;
        [SerializeField] private Image _thirdChecker;
        private int _gainedStarNumber => _thirdChecker.enabled ? 3 : _secondChecker.enabled ? 2 : _firstChecker.enabled ? 1 : 0;

        [Header("Hearts")]
        [SerializeField] private List<Image> _heartImagelist;
        private int _currentHP;
        [SerializeField] private Sprite _fullHeartSprite;
        [SerializeField] private Sprite _brokenHeartSprite;

        [Header("Clear")] public Image ClearStarImage;
        public List<Sprite> StarSpriteList;
        public TextMeshProUGUI GainedCoinText;
        public Image MemoryImage;
        public TextMeshProUGUI GainedMemoryText;

        private Character _character;
        public event Action OnDirectionChangeButtonClicked;

        private float _realHeight;
        
        private void Start()
        {
            _character = Character.instance;
            _currentHP = _character.CurrentHp;
            SoundToggle.isOn = GameManager.instance.VolumeSetting > 0;
            StageInfoText.text = $"스테이지 모드 {Stage.StageLevel+1}-{Stage.SubLevel+1}";
            _progressHeightImage.sprite = CharacterProfileSpriteList[(int)_character.Friend.Name];
            _realHeight = Stage.Spec.Height * GridTransform.GridSetting.GridHeight;

            ThunderEffect.OnThunderAttack += OnThunderAttack;
        }

        private void Update()
        {
            _progressHeightBar.value = _character.transform.position.y / _realHeight;
        }

        private void OnDestroy()
        {
            ThunderEffect.OnThunderAttack -= OnThunderAttack;
            if (InputManager.instance)
            InputManager.instance.InputAsset.UI.Cancel.performed -= Pause;
        }

        void OnThunderAttack(float duration)
        {
            _changeDirectionButton.interactable = false;
            _changeDirectionButton.image.color = new Color(1, 1, 1, 0.5f);;
            UpdateManager.instance.Delay(() =>
            {
                _changeDirectionButton.interactable = true;
                _changeDirectionButton.image.color = Color.white;
            }, duration);
        }

        public void CountDownFinish()
        {
            Stage.CountDownFinish();
            InputManager.instance.InputAsset.UI.Cancel.performed += Pause;
        }

        private void Pause(InputAction.CallbackContext context)
        {
            if (PauseUI.alpha < 1)
            {
                Pause();
            }
        }
        
        public void Pause()
        {
            PauseUI.alpha = 1;
            PauseUI.blocksRaycasts = true;
            MoonBunnyRigidbody.DisableAll();
            TimeUpdatable.GlobalSpeed = 0;
        }

        public void Unpause()
        {
            PauseUI.alpha = 0;
            PauseUI.blocksRaycasts = false;
            MoonBunnyRigidbody.EnableAll();
            TimeUpdatable.GlobalSpeed = 1;
        }

        public void SetScore(int score)
        {
            _scoreText.text = score.ToString();
            
            if (score >= Stage.Spec.ThirdStepScore)
            {
                _progressBar.value = 1f;
                Stage.GainedStar = 3;
            } else if (score >= Stage.Spec.SecondStepScore)
            {
                _progressBar.value = 0.62f;
                Stage.GainedStar = 2;
            } else if (score >= Stage.Spec.FirstStepScore)
            {
                _progressBar.value = 0.2f;
                Stage.GainedStar = 1;
            }

            if (score >= Stage.Spec.ThirdStepScore)
            {
                _thirdStar.sprite = _progressFullStar;
                _thirdChecker.enabled = true;
            }

            if (score >= Stage.Spec.SecondStepScore)
            {
                _secondStar.sprite = _progressFullStar;
                _secondChecker.enabled = true;
            }

            if (score >= Stage.Spec.FirstStepScore)
            {
                _firstStar.sprite = _progressFullStar;
                _firstChecker.enabled = true;
            }
        }

        public void ChangeDirectionButtonClicked()
        {
            OnDirectionChangeButtonClicked?.Invoke();
        }

        private bool _cleared = false;
        
        public void Clear()
        {
            _cleared = true;
            
            FadeIn(ClearUI);

            ClearStarImage.sprite = StarSpriteList[_gainedStarNumber];
            GainedCoinText.text = $"{Stage.GoldNumber} x {Stage.GoldMultiplier}";
            
            foreach (var friendName in EnumHelper.ClapValuesOfEnum<FriendName>(0))
            {
                if (Stage.CollectDict[friendName] > 0)
                {
                    MemoryImage.sprite = PreloadedResources.instance.MemorySpriteList[(int)friendName - 1];
                    GainedMemoryText.text = Stage.CollectDict[friendName].ToString();
                    break;
                }
            }
                
            SoundManager.instance.PlayClip(_clearAudioClip);
        }

        public void Fail()
        {
            FailUI.Open();
            
            SoundManager.instance.PlayClip(_failAudioClip);
        }

        public void Revive()
        {
            Stage.Revive();

            FailUI.OnExitButtonClicked();
            GameManager.instance.GoldNumber -= StageFailUI.REVIVE_GOLD_COST;
        }

        public void RetryButtonClicked()
        {
            TimeUpdatable.GlobalSpeed = 1;
            UpdateManager.instance.Clear();
            
            Canvas canvas = GetComponent<Canvas>();
            canvas.sortingOrder = 1;
            Scene startScene = SceneManager.GetActiveScene();
            CameraSetter previousCameraSetter = FindObjectOfType<CameraSetter>();
            previousCameraSetter.MainCamera.tag = "Untagged";
            previousCameraSetter.MainCamera.depth = 1;
            previousCameraSetter.Brain.enabled = false;
            Destroy(GameObject.FindWithTag("GlobalLight").gameObject);
            
            SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name, LoadSceneMode.Additive).completed += (ao) =>
            {
                Destroy(previousCameraSetter.AudioListener);

                Stage stage = GameManager.instance.Stage;

                Character character = Character.instance;

                if (FailUI.Boost.Checked)
                {
                    if (FailUI.Boost.BoostName == MagnetBoostEffect.BoostName)
                    {
                        stage.BoostEffectList.Add(new MagnetBoostEffect(character));
                    }
                    else
                    {
                        stage.BoostEffectList.Add(new StarCandyBoostEffect());
                    }
                }
                
                stage.gameObject.SetActive(false);
                character.gameObject.SetActive(false);
                    
                CameraSetter nextCameraSetter = Camera.main.GetComponentInParent<CameraSetter>();

                nextCameraSetter.MainCamera.GetComponentInParent<CameraSetter>().OnCameraSetFinish += () =>
                {
                    canvas.sortingOrder = -1;
                    SceneManager.UnloadSceneAsync(startScene).completed += (ao) =>
                    {
                        stage.gameObject.SetActive(true);
                        character.gameObject.SetActive(true);
                        nextCameraSetter.MainCamera.enabled = true;
                    };
                };
                nextCameraSetter.MainCamera.enabled = false;
            };
            
            GameManager.instance.GoldNumber -= BoostUI.S_ConsumingGold;
            BoostUI.S_ConsumingGold = 0;
            GameManager.instance.SaveProgress();
        }

        public void GoToLobbyButtonClicked()
        {
            Stage.OnGotoStageSelect();
            UpdateManager.instance.Clear();

            SceneManager.LoadSceneAsync(SceneName.Start).completed += (ao) =>
            {
                GameManager.instance.StartSceneUI.FriendSelectUI.OnExitButtonClicked(0);
                GameManager.instance.StartSceneUI.StageSelectUI.Open(0);
            };
        }

        public void OnSoundToggle()
        {
            GameManager.instance.VolumeSetting = SoundToggle.isOn ? 1 : 0;
        }

        public void LoseHP()
        {
            if (_currentHP <= 0) return;
            
            _currentHP--;
            _heartImagelist[_currentHP].sprite = _brokenHeartSprite;
        }

        public void GainHP()
        {
            if (_currentHP >= Friend.MaxHp)
            {
                return;
            }
            
            _heartImagelist[_currentHP].sprite = _fullHeartSprite;
            _currentHP++;
        }

        public void TutorialOn()
        {
            TutorialUI.Open(0);
        }
    }
}