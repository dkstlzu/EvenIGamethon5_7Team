using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

namespace EvenI7.Proto.UI
{
    public class ScoreUI : MonoBehaviour
    {
        private ProtoCharacter _protoCharacter;
        private TextMeshProUGUI _text;

        public string Text
        {
            get
            {
                return _text.text;
            }
            set
            {
                _text.text = value;
            }
        }

        private void Start()
        {
            _protoCharacter = GameObject.FindWithTag("Player").GetComponent<ProtoCharacter>();
            _protoCharacter.OnTakeItem += UpdateScore;
            _text = GetComponent<TextMeshProUGUI>();
            UpdateScore();
        }

        private void UpdateScore()
        {
            Text = "Score : " + _protoCharacter.Score.ToString();
        }
    }
}