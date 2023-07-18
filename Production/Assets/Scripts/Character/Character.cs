using System;
using dkstlzu.Utility;
using UnityEngine;

namespace MoonBunny
{
    public class Character : GridObject
    {
        // Hard Caching
        private static readonly int _jumpHash = Animator.StringToHash("FirstJumped");
        private static readonly int _fallingHash = Animator.StringToHash("isFalling");
        
        [SerializeField] private Animator _animator;
        [SerializeField] private SpriteRenderer _renderer;
        [SerializeField] private MoonBunnyRigidbody _rigidbody;
        
        public bool FirstJumped;
        public Friend Friend;

        private int _currentHP;
        public int CurrentHp
        {
            get => _currentHP;
            set
            {
                _currentHP = value;

                if (GameManager.instance != null)
                {
                    if (GameManager.instance.Stage != null)
                    {
                        GameManager.instance.Stage.UI.LoseHP();
                    }
                }
            }
        }

        public bool LookingRight => !_renderer.flipX;

        protected override void Awake()
        {
            base.Awake();
            
#if UNITY_EDITOR
            if (!UnityEditor.EditorApplication.isPlaying) return;
#endif
            
            _rigidbody.GridJumpVelocity = Friend.JumpPower;
        }

        private void Start()
        {
#if UNITY_EDITOR
            if (!UnityEditor.EditorApplication.isPlaying) return;
#endif
            CurrentHp = Friend.MaxHp;
        }

        protected override void Update()
        {
            base.Update();
#if UNITY_EDITOR
            if (!UnityEditor.EditorApplication.isPlaying) return;
#endif
            if (_rigidbody.isMovingToLeft)
            {
                _renderer.flipX = true;
            }
            else if (_rigidbody.isMovingToRight)
            {
                _renderer.flipX = false;
            }
            _animator.SetBool(_fallingHash, _rigidbody.isFalling);
        }

        public void StartJump()
        {
            if (FirstJumped || !_rigidbody.isGrounded) return;
            
            _rigidbody.Move(new Vector2Int(1, 4));
            _animator.SetBool(_jumpHash, true);
            FirstJumped = true;
        }

        public void FlipDirection()
        {
            _rigidbody.Move(new Vector2(-_rigidbody.Velocity.x, _rigidbody.Velocity.y));
        }

        public void Hit()
        {
            CurrentHp -= 1;
        }
    }
}