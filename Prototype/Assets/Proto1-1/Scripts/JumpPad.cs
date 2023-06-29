using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

namespace EvenI7.Proto1_1
{
    public class JumpPad : MonoBehaviour
    {
        private Proto1_1CharacterInput _proto11CharacterInput;
        public bool JumpPadIsFollowingTouch;
        public Animator AC;

        private readonly int _jumpPadAcHash = Animator.StringToHash("Jump");
        
        private void Start()
        {
            _proto11CharacterInput = GameObject.FindWithTag("Player").GetComponent<Proto1_1CharacterInput>();
            print("Jump pad Start");
            _proto11CharacterInput.OnIngameTouchPressed += SwitchJumpPadFollow;
            _proto11CharacterInput.OnIngameTouchUnpressed += SwitchJumpPadFollow;
        }

        void SwitchJumpPadFollow()
        {
            if (JumpPadIsFollowingTouch)
            {
                print("SwitchPadFollow off");
                _proto11CharacterInput.InputAsset.Ingame.TouchPosition.performed -= JumpPadPositionUpdate;
                JumpPadIsFollowingTouch = false;
            }
            else
            {
                print("SwitchPadFollow on");
                _proto11CharacterInput.InputAsset.Ingame.TouchPosition.performed += JumpPadPositionUpdate;
                JumpPadIsFollowingTouch = true;
            }
        }
        

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.tag == "Player")
            {
                AC.SetTrigger(_jumpPadAcHash);
                other.gameObject.GetComponent<Proto1_1Character>().JumpadJump(this);
            }
        }

        public void JumpPadPositionUpdate(InputAction.CallbackContext callbackContext)
        {
            Vector2 touchPosition = callbackContext.ReadValue<Vector2>();
            Vector2 screenSizeInWorld = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0)) - Camera.main.transform.position;
            // Vector2 screenSizeInWorld =  Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelWidth, Camera.main.pixelHeight, 0)) - Camera.main.transform.position;

            Vector2 touchWorldPosition = Camera.main.ScreenToWorldPoint(touchPosition);
            Vector2 touchLocalPosition = Camera.main.transform.InverseTransformPoint(touchWorldPosition);

            Vector3 clampedTouchWorldPosition = new Vector3(Mathf.Clamp(touchLocalPosition.x, -screenSizeInWorld.x, screenSizeInWorld.x), 
                                                            Mathf.Clamp(touchLocalPosition.y, -screenSizeInWorld.y, screenSizeInWorld.y),
                                                            10);
            
            print($"DB - touchPosition : {touchPosition}, screenSizeInWorld : {screenSizeInWorld}, touchWorldPosition : {touchWorldPosition}, touchLocalPosition {touchLocalPosition}, clampedTouchWorldPosition : {clampedTouchWorldPosition}");
            transform.localPosition = clampedTouchWorldPosition;
        }
    }
}
