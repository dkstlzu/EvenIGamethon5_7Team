using System;
using UnityEngine;

namespace MoonBunny.UIs
{
    public class TutorialUI : MonoBehaviour
    {
        private InputManager _inputManager;
        private void Awake()
        {
            _inputManager = GameObject.FindWithTag("GameController").GetComponent<InputManager>();
        }

        public void On()
        {
            _inputManager.InputAsset.Ingame.Disable();
        }

        public void Off()
        {
            _inputManager.InputAsset.Ingame.Enable();
        }
    }
}