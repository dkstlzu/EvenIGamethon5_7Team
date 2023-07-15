using System;
using DG.Tweening;
using MoonBunny.Dev;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MoonBunny.UIs
{
    public class StageUI : MonoBehaviour
    {
        public CanvasGroup PauseUI;

        [SerializeField] private TextMeshProUGUI _scoreText;
        [SerializeField] private Slider _progressBar;
        
        private Character _character;
        
        private void Start()
        {
            _character = GameObject.FindWithTag("Player").GetComponent<Character>();
        }

        public void Pause()
        {
            PauseUI.DOFade(1, 1);
        }

        public void Unpause()
        {
            PauseUI.DOFade(0, 1);
        }

        public void SetScore(int score)
        {
            _scoreText.text = score.ToString();
        }

        public void ChangeDirectionButtonClicked()
        {
            print("ChangeDirectionButtonClicked");
            if (!_character.FirstJumped) _character.StartJump();
            else _character.FlipDirection();
        }

        public void Clear()
        {
            MoonBunnyLog.print("Clear on ui");
        }

        public void Fail()
        {
            MoonBunnyLog.print("Fail on ui");
        }

        public void LoseHP()
        {
            MoonBunnyLog.print("Lost HP on ui");
        }
    }
}