using System;
using System.Collections;
using System.Collections.Generic;
using MoonBunny.Effects;
using UnityEngine;
using UnityEngine.Serialization;

namespace MoonBunny
{
    public class Character : GridObject
    {
        // Hard Caching
        private static readonly int _jumpHash = Animator.StringToHash("FirstJumped");
        private static readonly int _fallingHash = Animator.StringToHash("isFalling");

        [SerializeField] private Animator _animator;
        public SpriteRenderer Renderer;
        [SerializeField] private MoonBunnyRigidbody _rigidbody;

        public bool FirstJumped;
        public Friend Friend;
        public CircleCollider2D MagneticField;

        [SerializeField] private int _currentHP;

        public List<Buff> BuffList = new List<Buff>();
        public SpriteRenderer DebuffSpriteRenderer;

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

        public float InvincibleDuration = 3;
        public AnimationCurve InvincibleEffectCurve;

        public bool isIgnoringFlip { get; set; }

        public bool LookingRight => !Renderer.flipX;

        protected override void Awake()
        {
            base.Awake();

#if UNITY_EDITOR
            if (!UnityEditor.EditorApplication.isPlaying) return;
#endif
            CurrentHp = Friend.MaxHp;
            new MagnetEffect(MagneticField, Friend.MagneticPower).Effect();
        }

        private void Start()
        {
#if UNITY_EDITOR
            if (!UnityEditor.EditorApplication.isPlaying) return;
#endif
            GameManager.instance.Stage.UI.OnDirectionChangeButtonClicked += OnButtonClicked;
            _rigidbody.SetDefaultHorizontalSpeed(Friend.HorizontalSpeed);
            _rigidbody.SetBounciness(Friend.JumpPower);
            if (Friend.Name == FriendName.BlackSugar)
            {
                _rigidbody.CanDestroyObstaclesByStepping = true;
            }
        }

        protected override void Update()
        {
            base.Update();
#if UNITY_EDITOR
            if (!UnityEditor.EditorApplication.isPlaying) return;
#endif
            if (_rigidbody.isMovingToLeft)
            {
                Renderer.flipX = true;
            }
            else if (_rigidbody.isMovingToRight)
            {
                Renderer.flipX = false;
            }

            _animator.SetBool(_fallingHash, _rigidbody.isFalling);
        }

        private void OnButtonClicked()
        {
            if (isIgnoringFlip) return;

        if (!FirstJumped) StartJump();
            else FlipDirection();
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

        public void Hit(Obstacle by)
        {
            CurrentHp -= 1;
            GameManager.instance.Stage.UI.LoseHP();
        }


        public void GetHeart()
        {
            if (_currentHP >= Friend.MaxHp) return;
            
            CurrentHp += 1;
            GameManager.instance.Stage.UI.GainHP();
        }
    }
}