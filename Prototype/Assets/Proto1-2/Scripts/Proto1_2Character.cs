using UnityEngine;
using UnityEngine.Serialization;

namespace EvenI7.Proto1_2
{
    public class Proto1_2Character : MonoBehaviour
    {
        public Rigidbody2D Rigidbody;
        public float MoveSpeed;
        public float JumpPower;
        public bool MovingToRight;
        
        private Vector2 _lastVelocity;
        private float _goundedCheckDistance = 0.1f;
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
        
        public void BouncyPlatformJump(float jumpPower)
        {
            Rigidbody.velocity = new Vector2(_lastVelocity.x, jumpPower);
        }

        public void Jump()
        {
            Rigidbody.velocity = new Vector2(MoveSpeed, JumpPower);
        }

        public void SwitchDirection()
        {
            Rigidbody.velocity = new Vector2(-_lastVelocity.x, _lastVelocity.y);
        }
    }
}