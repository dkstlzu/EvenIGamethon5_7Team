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
        public MoonBunnyRigidbody Rigidbody;

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
            new MagnetEffect(this, Friend.MagneticPower).Effect();

            isIgnoringFlip = false;
        }

        private void Start()
        {
#if UNITY_EDITOR
            if (!UnityEditor.EditorApplication.isPlaying) return;
#endif
            GameManager.instance.Stage.UI.OnDirectionChangeButtonClicked += OnButtonClicked;
            Rigidbody.SetDefaultHorizontalSpeed(Friend.HorizontalSpeed);
            Rigidbody.SetBounciness(Friend.JumpPower / 3);
            if (Friend.Name == FriendName.BlackSugar)
            {
                Rigidbody.CanDestroyObstaclesByStepping = true;
            }
        }

        protected override void Update()
        {
            base.Update();
#if UNITY_EDITOR
            if (!UnityEditor.EditorApplication.isPlaying) return;
#endif
            if (Rigidbody.isMovingToLeft)
            {
                Renderer.flipX = true;
            }
            else if (Rigidbody.isMovingToRight)
            {
                Renderer.flipX = false;
            }

            _animator.SetBool(_fallingHash, Rigidbody.isFalling);
        }

        private void OnButtonClicked()
        {
            if (isIgnoringFlip) return;

            if (!FirstJumped) StartJump();
            else FlipDirection();
        }

        public void StartJump(int jumpPower = 4)
        {
            if (FirstJumped) return;

            Rigidbody.Move(new Vector2Int(1, jumpPower));
            _animator.SetBool(_jumpHash, true);
            FirstJumped = true;
        }

        public void FlipDirection()
        {
            Rigidbody.Move(new Vector2(-Rigidbody.Velocity.x, Rigidbody.Velocity.y));
        }

        public void Hit(Obstacle by)
        {
            if (CurrentHp <= 0) return;
            
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