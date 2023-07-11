using System;
using MoonBunny;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace MoonBunny
{
    public class Character : GridObject
    {
        // Hard Caching
        private static readonly int _hitHash = Animator.StringToHash("Hit");
        
        [SerializeField] private Animator animator;
        
        public bool MovingToRight;
        public bool FirstJumped;
        public Friend Friend;

        private IsGrounded _isGrounded;

        public int CurrentHp
        {
            get => Friend.CurrentHp;
            set => Friend.CurrentHp = value;
        }

        protected void Awake()
        {
            _isGrounded = new IsGrounded();
            _isGrounded.Type = GroundCheckType.Line;
            _isGrounded.TargetTransform = transform;
        }

        private Vector2 _lastVelocity;

        private void FixedUpdate()
        {
            MovingToRight = (_lastVelocity.x >= 0);
        }

        public void StartJump()
        {
            if (FirstJumped) return;
        }

        public void Hit(int damage)
        {
            CurrentHp -= damage;
        }

        // public void Enable()
        // {
        //     FirstJumped = false;
        //     gameObject.SetActive(true);
        //     transform.position = GameManager.instance.StartPosition.position;
        // }
        //
        // public void Disable()
        // {
        //     gameObject.SetActive(false);
        // }

        private void OnDrawGizmos()
        {
            if (_isGrounded == null) return;

            if (_isGrounded.Type == GroundCheckType.Line)
            {
                Gizmos.DrawLine(transform.position, transform.position + Vector3.down * _isGrounded.CheckDistance);
            } else if (_isGrounded.Type == GroundCheckType.Box)
            {
                Gizmos.DrawCube(transform.position, Vector3.one);
            }
        }
    }
}