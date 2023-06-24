using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Character = EvenI7.ProtoScreenSplit.ProtoScreenSplitCharacter;

namespace EvenI7.ProtoScreenSplit
{
    public class ProtoScreenSplitInputManager : MonoBehaviour
    {
        public ProtoScreenSplitInputAsset InputAsset;
        public Character Character;

        
        private void Awake()
        {
            InputAsset = new ProtoScreenSplitInputAsset();
            InputAsset.Enable();

            InputAsset.Ingame.TouchPressed.performed += OnTouchScreen;
        }

        private void OnTouchScreen(InputAction.CallbackContext obj)
        {
            ScreenSplit.ScreenSide side = ScreenSplit.instance.GetSide(InputAsset.Ingame.TouchPosition.ReadValue<Vector2>());
            print($"On Touch Screen {side}");
            Character targetCharacter = ScreenSplit.instance.GetScreen(side).Character;
            if (!targetCharacter.FirstJump) targetCharacter.StartJump();
            else targetCharacter.SwitchMoveDirection();
        }
    }
}