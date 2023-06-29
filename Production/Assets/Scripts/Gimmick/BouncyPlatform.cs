using System;
using dkstlzu.Utility;
using UnityEngine;
using Random = UnityEngine.Random;

namespace MoonBunny
{
    public class BouncyPlatform : Platform
    {
        private static readonly int JumpHash = Animator.StringToHash("Jump");

        [SerializeField] private Animator _animator;
        [SerializeField] private Rigidbody2D _rigidbody2D;
        [Range(0, 1)] public float HorizontalDampingSpeed;
        public float JumpPower;

        private void FixedUpdate()
        {
            if (_rigidbody2D.velocity.magnitude > 0)
            _rigidbody2D.velocity = new Vector2( Mathf.Lerp(_rigidbody2D.velocity.x, 0, HorizontalDampingSpeed), _rigidbody2D.velocity.y);
        }

        public void PushedOut(Character by)
        {
            Vector2 direction = by.transform.position.x > transform.position.x ? Vector2.left : Vector2.right;
            
            PushedOut(by.Friend.PushingPlatformPower, direction);
        }

        void PushedOut(float power, Vector2 direction)
        {
            _rigidbody2D.AddForce(direction * power, ForceMode2D.Impulse);
        }
    }
}