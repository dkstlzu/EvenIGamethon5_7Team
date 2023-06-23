using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace EvenI7.Proto_Swiping
{
    public class SwipingCharacterInput : MonoBehaviour
    {
        public Proto_Swiping_InputAsset InputAsset;
        public Swiping SwipingLine;
        public SwipingCharacter Character;
        public bool isSwiping;

        private Vector2 _swipeStartingPosition;
        private Vector2 _swipeEndingPosition;
        
        private void Awake()
        {
            InputAsset = new Proto_Swiping_InputAsset();
            InputAsset.Enable();

            InputAsset.Ingame.TouchPressed.started += StartSwipe;
            InputAsset.Ingame.TouchPressed.canceled += EndSwipe;
        }

        private void Update()
        {
            if (isSwiping)
            {
                Vector2 swipingPosition =
                    Camera.main.ScreenToWorldPoint(InputAsset.Ingame.TouchPosition.ReadValue<Vector2>());
                SwipingLine.SetUp(swipingPosition, _swipeStartingPosition);
            }
        }

        private void StartSwipe(InputAction.CallbackContext obj)
        {
            _swipeStartingPosition = Camera.main.ScreenToWorldPoint(InputAsset.Ingame.TouchPosition.ReadValue<Vector2>());
            SwipingLine.gameObject.SetActive(true);
            SwipingLine.SetUp(_swipeStartingPosition, _swipeStartingPosition);
            isSwiping = true;
        }
        
        private void EndSwipe(InputAction.CallbackContext obj)
        {
            _swipeEndingPosition = Camera.main.ScreenToWorldPoint(InputAsset.Ingame.TouchPosition.ReadValue<Vector2>());
            SwipingLine.gameObject.SetActive(false);
            isSwiping = false;
            Vector2 direction = _swipeStartingPosition - _swipeEndingPosition;
            float power = direction.magnitude;
            direction.Normalize();
            
            Character.Jump(direction, power);
        }
    }
}