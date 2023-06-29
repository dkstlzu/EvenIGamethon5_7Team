using System;
using EvenI7.Proto1_2;
using UnityEngine;

namespace EvenI7.Proto_Swiping
{
    public class SwipingCharacter : MonoBehaviour
    {
        public Rigidbody2D Rigidbody;
        public float JumpPower;
        
        private Vector2 _lastVelocity;
        private void FixedUpdate()
        {
            _lastVelocity = Rigidbody.velocity;
        }
        
        public void Jump(Vector2 direction, float power)
        {
            Rigidbody.velocity = direction * power * JumpPower;
        }

        public void BouncyJump(float jumpPower)
        {
            Rigidbody.velocity = new Vector2(_lastVelocity.x, jumpPower);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Platform"))
            {
                BouncyPlatform bp;
                if (other.gameObject.TryGetComponent<BouncyPlatform>(out bp))
                {
                    BouncyJump(bp.JumpPower);
                }
            }

            if (other.gameObject.layer == LayerMask.NameToLayer("Item"))
            {
                other.GetComponent<Item>().Taken();
            }
            
            if (other.gameObject.layer == LayerMask.NameToLayer("Obstacle"))
            {
                Destroy(other.gameObject);
            }
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            print("Swiping Collision Test : " + other.gameObject.name);
            SideWallBound sw;
            if (other.gameObject.TryGetComponent<SideWallBound>(out sw))
            {
                Rigidbody.velocity = new Vector2(-_lastVelocity.x, _lastVelocity.y);
            }
        }
    }
}