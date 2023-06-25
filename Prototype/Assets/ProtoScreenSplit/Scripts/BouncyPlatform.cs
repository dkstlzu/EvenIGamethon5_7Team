using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace EvenI7.ProtoScreenSplit
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class BouncyPlatform : Platform
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private Rigidbody2D _rigidbody2D;
        public float DampingSpeed;
        public float JumpPower;
        private static readonly int JumpHash = Animator.StringToHash("Jump");

        private void Reset()
        {
            GetComponent<BoxCollider2D>().isTrigger = true;
        }

        private void FixedUpdate()
        {
            _rigidbody2D.velocity = new Vector2( Mathf.Lerp(_rigidbody2D.velocity.x, 0, DampingSpeed), _rigidbody2D.velocity.y);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            Bouncable bouncable;

            if (other.TryGetComponent<Bouncable>(out bouncable))
            {
                bouncable.BounceUp(JumpPower);
                _animator.SetTrigger(JumpHash);
            }
        }

        public void PushedOut(ProtoScreenSplitCharacter by)
        {
            Vector2 direction;
            if (by.transform.position.x > transform.position.x)
            {
                direction = Vector2.left;
            }
            else
            {
                direction = Vector2.right;
            }
            
            PushedOut(by.FriendCharacter.PushingPlatformPower, direction);
        }

        public void PushedOut(float power, Vector2 direction)
        {
            _rigidbody2D.AddForce(direction * power, ForceMode2D.Impulse);
        }
    }
}