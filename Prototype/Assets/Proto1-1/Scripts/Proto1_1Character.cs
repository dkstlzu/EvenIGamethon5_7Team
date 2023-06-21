using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace EvenI7.Proto1_1
{
    public class Proto1_1Character : MonoBehaviour
    {
        public Rigidbody2D Rigidbody;
        [FormerlySerializedAs("CharacterInput")] public Proto1_1CharacterInput Proto11CharacterInput;
        [Range(-100, 100)] public float JumpPowerMin;
        [Range(-100, 100)] public float JumpPowerMax;
        [Range(0, 10)] public float SideJumpPower;

        private Vector2 _lastVelocity;
        private void FixedUpdate()
        {
            _lastVelocity = Rigidbody.velocity;
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Item"))
            {
                other.gameObject.GetComponent<Item>().Taken();
            }
            
            if (other.gameObject.layer == LayerMask.NameToLayer("Obstacle"))
            {
                print("Hit By Obstacle");
                Destroy(other.gameObject);
            }
        }

        public void JumpadJump(JumpPad pad)
        {
            Rigidbody.velocity = new Vector2(0, Mathf.Abs(_lastVelocity.y));
            Vector2 offset = transform.position - pad.transform.position;
            Rigidbody.AddForce(Vector2.right * offset.x, ForceMode2D.Impulse);

            if (Proto11CharacterInput.InputAsset.Ingame.TouchPressed.IsPressed())
            {
                Vector2 delta = Proto11CharacterInput.InputAsset.Ingame.TouchDelta.ReadValue<Vector2>();
                float yDelta = Mathf.Clamp(delta.y, JumpPowerMin, JumpPowerMax);
                Rigidbody.AddForce(Vector2.up * yDelta, ForceMode2D.Impulse);
            }
        }

        public void SideWallBounce()
        {
            Rigidbody.velocity = new Vector2(-_lastVelocity.x, _lastVelocity.y);
        }
    }
}