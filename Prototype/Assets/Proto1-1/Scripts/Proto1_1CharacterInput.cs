using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace EvenI7.Proto1_1
{
    public class Proto1_1CharacterInput : MonoBehaviour
    {
        public Proto1_1InputAsset InputAsset;


        public event Action OnIngameTouchPressed;
        public event Action OnIngameTouchUnpressed;
        
        private void Awake()
        {
            InputAsset = new Proto1_1InputAsset();
            InputAsset.Enable();

            InputAsset.Ingame.TouchPressed.performed += TouchPressed;
            InputAsset.Ingame.TouchPressed.canceled += TouchUnpressed;
        }

        private void OnEnable()
        {
            InputAsset.Enable();
        }

        private void OnDisable()
        {
            InputAsset.Disable();
        }

        void TouchPressed(InputAction.CallbackContext callbackContext)
        {
            OnIngameTouchPressed?.Invoke();
        }

        void TouchUnpressed(InputAction.CallbackContext callbackContext)
        {
            OnIngameTouchUnpressed?.Invoke();
        }
    }
}