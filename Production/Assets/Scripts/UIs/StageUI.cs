using System;
using System.Collections.Generic;
using DG.Tweening;
using MoonBunny.Dev;
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
        [SerializeField] private TextMeshProUGUI _scoreText;
        
        [Header("Progress")]
        [SerializeField] private Slider _progressBar;

        [SerializeField] private Image _firstChecker;
        [SerializeField] private Image _secondChecker;
        [SerializeField] private Image _thirdChecker;

        [Header("Hearts")]
        [SerializeField] private List<Image> _heartImagelist;
        private int _currentHP;
        [SerializeField] private Sprite _fullHeartSprite;
        [SerializeField] private Sprite _brokenHeartSprite;
        
        private Character _character;
        
        private void Start()
        {
            _character = GameObject.FindWithTag("Player").GetComponent<Character>();
            _currentHP = _character.CurrentHp;
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
                _thirdChecker.enabled = true;
                _progressBar.value = 1f;
            } else if (score >= Stage.Spec.SecondStepScore)
            {
                _secondChecker.enabled = true;
                _progressBar.value = 0.69f;

            } else if (score >= Stage.Spec.FirstStepScore)
            {
                _firstChecker.enabled = true;
                _progressBar.value = 0.39f;
            } 
        }

        public void ChangeDirectionButtonClicked()
        {
            if (!_character.FirstJumped) _character.StartJump();
            else _character.FlipDirection();
        }

        public void Clear()
        {
            ClearUI.DOFade(1, 2);
            ClearUI.blocksRaycasts = true;
            SoundManager.instance.PlayClip(_clearAudioClip);
        }

        public void Fail()
        {
            FailUI.DOFade(1, 2);
            FailUI.blocksRaycasts = true;
            SoundManager.instance.PlayClip(_failAudioClip);
        }

        public void GoToLobbyButtonClicked()
        {
            SceneManager.LoadScene(SceneName.Start);
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