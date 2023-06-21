using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace EvenI7.Proto
{
    public class ProtoCharacterInput : MonoBehaviour
    {
        public CharacterInputAsset InputActionAsset;
        [FormerlySerializedAs("Character")] public ProtoCharacter ProtoCharacter;

        private void Awake()
        {
            InputActionAsset = new CharacterInputAsset();

            InputActionAsset.Ingame.JumpTest.performed += Jump;
            InputActionAsset.Ingame.Tap.performed += Jump;
        }

        private void OnEnable()
        {
            InputActionAsset.Enable();
        }

        private void OnDisable()
        {
            InputActionAsset.Disable();
        }
        
        
        public void Jump(InputAction.CallbackContext context)
        {
            if (ProtoCharacter.JumpCount <= 0) return;
            if (ProtoCharacter.MaxJumpCount > ProtoCharacter.JumpCount && !ProtoCharacter.DoubleJumpEnable) return;
            
            ProtoCharacter.JumpCount--;
            
            ProtoCharacter.Rigidbody.AddForce(Vector2.up * ProtoCharacter.JumpPower, ForceMode2D.Impulse);
            ProtoCharacter.AC.SetTrigger(ProtoCharacter.JumpAcHash);
        }
    }
}