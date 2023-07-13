using System;
using UnityEngine;

namespace MoonBunny.UIs
{
    public class StageUI : MonoBehaviour
    {
        public GameObject PauseUI;

        private Character _character;
        
        private void Start()
        {
// #if UNITY_EDITOR
//             GameObject.FindWithTag("GameController").GetComponent<InputManager>().OnESCInputPerformed += Pause;
// #endif
            _character = GameObject.FindWithTag("Player").GetComponent<Character>();
        }

        private void OnDestroy()
        {
// #if UNITY_EDITOR
//             GameObject.FindWithTag("GameController").GetComponent<InputManager>().OnESCInputPerformed -= Pause;
// #endif
        }

        public void Pause()
        {
            PauseUI.SetActive(true);
        }

        public void Unpause()
        {
            PauseUI.SetActive(false);
        }

        public void ChangeDirectionButtonClicked()
        {
            print("ChangeDirectionButtonClicked");
            if (!_character.FirstJumped) _character.StartJump();
            else _character.FlipDirection();
        }
    }
}