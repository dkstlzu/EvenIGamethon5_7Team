using System;
using TMPro;
using UnityEngine;

namespace EvenI7.ProtoScreenSplit
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class ProtoScreenSplitCharacter : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D rigidbody;
        [SerializeField] private Animator animator;
        public bool MovingToRight;
        public bool FirstJump;
        public float FirstJumpPower;
        public FriendCharacter FriendCharacter;

        public int CurrentHp
        {
            get => FriendCharacter.CurrentHp;
            set => FriendCharacter.CurrentHp = value;
        }

        private int _itemLayer;
        private int _obstacleLayer;
        private int _platformLayer;
        private int _friendLayer;
        private int _hitHash;


        private void Reset()
        {
            rigidbody = GetComponent<Rigidbody2D>();
        }

        private void Awake()
        {
            _itemLayer = LayerMask.NameToLayer("Item");
            _obstacleLayer = LayerMask.NameToLayer("Obstacle");
            _platformLayer = LayerMask.NameToLayer("Platform");
            _friendLayer = LayerMask.NameToLayer("Friend");
            _hitHash  = Animator.StringToHash("Hit");
            
            FriendCharacter.LoadSprites();
        }

        private Vector2 _lastVelocity;

        private void FixedUpdate()
        {
            _lastVelocity = rigidbody.velocity;
            MovingToRight = (_lastVelocity.x >= 0);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            int layer = other.gameObject.layer;

            if (layer == _itemLayer)
            {
                other.GetComponent<Item>().Taken();
            } else if (layer == _obstacleLayer)
            {
                Hit(other.GetComponent<Obstacle>().Damage);
            } else if (layer == _platformLayer)
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

        public void SwitchMoveDirection()
        {
            rigidbody.velocity = new Vector2(-_lastVelocity.x, _lastVelocity.y);
        }

        public void StartJump()
        {
            if (FirstJump) return;
            Jump(FirstJumpPower);
            FirstJump = true;
        }

        public void Jump(float jumpPower)
        {
            float direction = MovingToRight ? FriendCharacter.HorizontalSpeed : -FriendCharacter.HorizontalSpeed;
            rigidbody.velocity = new Vector2(direction, jumpPower);
        }

        public void BouncyJump(float bouncyPower)
        {
            Jump(FriendCharacter.JumpPower + bouncyPower);
        }

        public void Hit(int damage)
        {
            CurrentHp -= damage;
        }
    }
}