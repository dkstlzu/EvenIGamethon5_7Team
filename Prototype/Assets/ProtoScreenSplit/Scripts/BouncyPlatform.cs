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

        public void PushedOut(float power)
        {
            int coef;
            if (Random.value > 0.5)
            {
                coef = 1;
            }
            else
            {
                coef = -1;
            }
            
            _rigidbody2D.AddForce(Vector2.right * coef * power, ForceMode2D.Impulse);
        }
    }
}