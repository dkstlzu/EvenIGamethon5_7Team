using System;
using MoonBunny;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace MoonBunny
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Character : MonoBehaviour
    {
        // Hard Caching
        private static readonly int _itemLayer = 7;
        private static readonly int _obstacleLayer = 8;
        private static readonly int _platformLayer = 9;
        private static readonly int _friendLayer = 10;
        private static readonly int _hitHash = Animator.StringToHash("Hit");
        
        [SerializeField] private Rigidbody2D _rigidbody;
        [SerializeField] private Animator animator;
        public Bouncable Bouncable; 
        
        public bool MovingToRight;
        public bool FirstJumped;
        public Friend Friend;

        private IsGrounded _isGrounded;

        public int CurrentHp
        {
            get => Friend.CurrentHp;
            set => Friend.CurrentHp = value;
        }

        private void Reset()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
        }

        private void Awake()
        {
            _isGrounded = new IsGrounded();
            _isGrounded.Type = GroundCheckType.Line;
            _isGrounded.TargetTransform = transform;
        }

        private Vector2 _lastVelocity;

        private void FixedUpdate()
        {
            _lastVelocity = _rigidbody.velocity;
            MovingToRight = (_lastVelocity.x >= 0);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            int layer = other.gameObject.layer;

            /*if (layer == _itemLayer)
            {
                other.GetComponent<Item>().Taken();
            } else if (layer == _obstacleLayer)
            {
                Hit(other.GetComponent<Obstacle>().Damage);
            } else */if (layer == _platformLayer)
            {
                var platform = other.GetComponent<BouncyPlatform>();
                BouncyJump(platform.JumpPower);
                platform.PushedOut(this);
            } else if (layer == _friendLayer)
            {
                var friend = other.GetComponent<FriendCollectable>();
                print($"Hello Firend! {friend.Name}");

                ScreenSplit.instance.AddNewScreen(friend.Name);
            }
        }

        public void StartJump()
        {
            if (FirstJumped) return;
            Jump(Friend.StartJumpHorizontalSpeed, Friend.StartJumpVerticalSpeed);
        }

        public void Jump(float horizontalSpeed, float verticalSpeed)
        {
            if (!_isGrounded) return;
            
            _rigidbody.velocity = new Vector2(horizontalSpeed, verticalSpeed);
        }

        public void BouncyJump(float bouncyPower)
        {
            FirstJumped = true;

            Jump(_lastVelocity.x, Friend.BouncyPower + bouncyPower);
        }

        public void Hit(int damage)
        {
            CurrentHp -= damage;
        }

        public void Enable()
        {
            FirstJumped = false;
            gameObject.SetActive(true);
            transform.position = GameManager.instance.StartPosition.position;
        }

        public void Disable()
        {
            gameObject.SetActive(false);
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