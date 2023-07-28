using System;
using System.Collections.Generic;
using System.Net.Sockets;
using DG.Tweening;
using dkstlzu.Utility;
using MoonBunny.Dev;
using MoonBunny.Effects;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace MoonBunny.UIs
{
    public class StageUI : MonoBehaviour
    {
        public CanvasGroup PauseUI;
        public CanvasGroup FailUI;
        public CanvasGroup ClearUI;

        [SerializeField] private AudioClip _clearAudioClip;
        [SerializeField] private AudioClip _failAudioClip;

        public Stage Stage;
        [SerializeField] private Button _changeDirectionButton;

        [Header("Progress")]
        [SerializeField] private Slider _progressBar;
        [SerializeField] private TextMeshProUGUI _scoreText;

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
        public TextMeshProUGUI ClearScoreText;
        
        private Character _character;
        public event Action OnDirectionChangeButtonClicked;
        
        private void Start()
        {
            _character = GameObject.FindWithTag("Player").GetComponent<Character>();
            _currentHP = _character.CurrentHp;

            ThunderEffect.OnThunderAttack += OnThunderAttack;
        }

        private void OnDestroy()
        {
            ThunderEffect.OnThunderAttack -= OnThunderAttack;
        }

        void OnThunderAttack(float duration)
        {
            _changeDirectionButton.interactable = false;
            CoroutineHelper.Delay(() => _changeDirectionButton.interactable = true, duration);
        }

        public void CountDownFinish()
        {
            Stage.CountDownFinish();
        }
        
        public void Pause()
        {
            PauseUI.alpha = 1;
            PauseUI.blocksRaycasts = true;
            MoonBunnyRigidbody.DisableAll();
        }

        public void Unpause()
        {
            PauseUI.alpha = 0;
            PauseUI.blocksRaycasts = false;
            MoonBunnyRigidbody.EnableAll();
        }

        public void SetScore(int score)
        {
            _scoreText.text = score.ToString();
            
            if (score >= Stage.Spec.ThirdStepScore)
            {
                _progressBar.value = 1f;
            } else if (score >= Stage.Spec.SecondStepScore)
            {
                _progressBar.value = 0.62f;

            } else if (score >= Stage.Spec.FirstStepScore)
            {
                _progressBar.value = 0.2f;
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

        public void Clear()
        {
            ClearUI.DOFade(1, 2);
            ClearUI.blocksRaycasts = true;

            ClearStarImage.sprite = StarSpriteList[_gainedStarNumber];
            ClearScoreText.text = _scoreText.text;
                
            SoundManager.instance.PlayClip(_clearAudioClip);
        }

        public void Fail()
        {
            FailUI.DOFade(1, 2);
            FailUI.blocksRaycasts = true;
            SoundManager.instance.PlayClip(_failAudioClip);
        }

        public void RetryButtonClicked()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        public void GoToLobbyButtonClicked()
        {
            SceneManager.LoadSceneAsync(SceneName.Start).completed += (ao) =>
            {
                GameManager.instance.StartSceneUI.FriendSelectCanvasGroup.alpha = 0;
                GameManager.instance.StartSceneUI.FriendSelectCanvasGroup.blocksRaycasts = false;
                GameManager.instance.StartSceneUI.StageSelectCanvasGroup.alpha = 1;
                GameManager.instance.StartSceneUI.StageSelectCanvasGroup.blocksRaycasts = true;
            };
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
    }
}