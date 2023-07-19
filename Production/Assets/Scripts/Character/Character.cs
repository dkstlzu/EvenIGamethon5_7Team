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
        [SerializeField] private CircleCollider2D _magneticField;
        [SerializeField] private AudioClip _jumpAudioClip;

        private int _currentHP;
        public int CurrentHp
        {
            get => _currentHP;
            set
            {
                _currentHP = value;
                if (_currentHP <= 0)
                {
                    GameManager.instance.Stage.Fail();
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
            
            _rigidbody.DefaultHorizontalSpeed = Friend.HorizontalSpeed;
            _rigidbody.BouncyRatio = Friend.JumpPower;
            CurrentHp = Friend.MaxHp;
            BouncyPlatformCollision.JumpAudioClip = _jumpAudioClip;
        }

        private void Start()
        {
#if UNITY_EDITOR
            if (!UnityEditor.EditorApplication.isPlaying) return;
#endif
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

        public void SetMagneticPower(float power, float duration)
        {
            float previousPower = _magneticField.radius;
            
            Friend.MagneticPower += power;
            _magneticField.radius += power;
            
            CoroutineHelper.Delay(() =>
            {
                Friend.MagneticPower = previousPower;
                _magneticField.radius = previousPower;
            }, duration);
        }

        public void Hit()
        {
            CurrentHp -= 1;
            if (GameManager.instance != null)
            {
                if (GameManager.instance.Stage != null)
                {
                    GameManager.instance.Stage.UI.LoseHP();
                }
            }
        }

        public void GetHeart()
        {
            if (_currentHP >= Friend.MaxHp) return;
            
            CurrentHp += 1;
            if (GameManager.instance != null)
            {
                if (GameManager.instance.Stage != null)
                {
                    GameManager.instance.Stage.UI.GainHP();
                }
                    
            }
        }
    }
}