using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MoonBunny
{
    public class InputManager : MonoBehaviour
    {
        public MoonBunnyInputAsset InputAsset;
#if UNITY_EDITOR
        public InputAction ESCInputAction;

        public event Action OnESCInputPerformed;
#endif

        private Character _character;
        private void Awake()
        {
            InputAsset = new MoonBunnyInputAsset();
            InputAsset.Enable();
            
#if UNITY_EDITOR
            ESCInputAction.Enable();
            ESCInputAction.performed += (ctx) => OnESCInputPerformed?.Invoke();
#endif
            
            InputAsset.Ingame.TouchPressed.performed += OnTouchScreen;
            DisableIngameInput();

            GameManager.instance.OnStageSceneLoaded += (stageName) => EnableIngameInput();
            GameManager.instance.OnStageSceneUnloaded += (stageName) => DisableIngameInput();
        }

        public void EnableIngameInput()
        {
            print("IngameInput Enabled");
            InputAsset.Ingame.Enable();
            _character = GameObject.FindWithTag("Player").GetComponent<Character>();
        }

        public void DisableIngameInput()
        {
            print("IngameInput Disabled");
            InputAsset.Ingame.Disable();
        }

        private void OnDestroy()
        {
            InputAsset.Ingame.TouchPressed.performed -= OnTouchScreen;
        }

        private void OnTouchScreen(InputAction.CallbackContext obj)
        {
            print("TouchTouch");
            // if (!_character.FirstJumped) _character.StartJump();
        }
    }
}