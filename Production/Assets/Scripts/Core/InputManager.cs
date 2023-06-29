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

        private void Start()
        {
            InputAsset = new MoonBunnyInputAsset();
            InputAsset.Enable();
#if UNITY_EDITOR
            ESCInputAction.Enable();
            ESCInputAction.performed += (ctx) => OnESCInputPerformed?.Invoke();
#endif
            
            InputAsset.Ingame.TouchPressed.performed += OnTouchScreen;
        }

        private void OnEnable()
        {
            InputAsset.Enable();
        }

        private void OnDisable()
        {
            InputAsset.Disable();
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