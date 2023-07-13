using UnityEditor;
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

        private IsGrounded _isGrounded;

        public int CurrentHp
        {
            get => Friend.CurrentHp;
            set => Friend.CurrentHp = value;
        }

        public bool LookingRight => !_renderer.flipX;

        protected override void Awake()
        {
            base.Awake();

            CurrentHp = Friend.MaxHp;
            
            _isGrounded = new IsGrounded();
            _isGrounded.Type = GroundCheckType.Line;
            _isGrounded.TargetTransform = transform;
        }

        protected override void Update()
        {
            base.Update();
#if UNITY_EDITOR
            if (!EditorApplication.isPlaying) return;
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
            if (FirstJumped) return;
            
            _rigidbody.Jump();
            _animator.SetBool(_jumpHash, true);
            FirstJumped = true;
        }

        public void FlipDirection()
        {
            _rigidbody.FlipXDirection();
        }

        public void Hit()
        {
            CurrentHp -= 1;
        }

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