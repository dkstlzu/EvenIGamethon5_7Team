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

        private void Awake()
        {
            InputAsset = new MoonBunnyInputAsset();
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
            InputAsset.Ingame.Enable();
        }

        public void DisableIngameInput()
        {
            InputAsset.Ingame.Disable();
        }

        private void OnDestroy()
        {
            InputAsset.Ingame.TouchPressed.performed -= OnTouchScreen;
        }

        private void OnTouchScreen(InputAction.CallbackContext obj)
        {
            ScreenSplit.ScreenSide side = ScreenSplit.instance.GetSide(InputAsset.Ingame.TouchPosition.ReadValue<Vector2>());
            Character targetCharacter = ScreenSplit.instance.GetScreen(side).Character;
            if (!targetCharacter.FirstJumped) targetCharacter.StartJump();
            else targetCharacter.Bouncable.BounceX();
        }
    }
}