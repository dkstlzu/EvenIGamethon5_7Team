using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace EvenI7.Proto1_2
{
    public class Proto1_2CharacterInput : MonoBehaviour
    {
        private Proto1_2InputAsset InputAsset;
        public Proto1_2Character Character;

        private void Awake()
        {
            InputAsset = new Proto1_2InputAsset();
            
            InputAsset.Enable();
            InputAsset.Ingame.Start.performed += OnStart;
            InputAsset.Ingame.TouchPressed.performed += OnTouch;
        }

        private void OnTouch(InputAction.CallbackContext obj)
        {
            Character.SwitchDirection();
        }

        private void OnStart(InputAction.CallbackContext obj)
        {
            Character.Jump();
            InputAsset.Ingame.Start.performed -= OnStart;
        }

        private void OnEnable()
        {
            InputAsset.Enable();
        }

        private void OnDisable()
        {
            InputAsset.Disable();
        }
    }
}