using System;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;

namespace EvenI7.Proto
{
    public class ProtoCharacter : MonoBehaviour
    {
        public Rigidbody2D Rigidbody;
        public SpriteRenderer CharacterRenderer;
        public Animator AC;
        
        public readonly int GroundedAcHash = Animator.StringToHash("isGrounded");
        public readonly int FallingAcHash = Animator.StringToHash("isFalling");
        public readonly int JumpAcHash = Animator.StringToHash("Jump");
        public readonly int RunningAcHash = Animator.StringToHash("isRunning");
        public readonly int HitAcHash = Animator.StringToHash("Hit");
        public readonly int DieAcHash = Animator.StringToHash("Die");
        
        public bool DoubleJumpEnable;
        public float JumpPower;
        public int MaxJumpCount;
        public int JumpCount;

        public int MaxHP;
        public int CurrentHP;

        public int Score;
        public bool isAlive;

        [SerializeField] private bool _isGrounded = false;
        public bool isGrounded
        {
            get
            {
                return _isGrounded;
            }
            set
            {
                _isGrounded = value;
                if (value)
                {
                    AC.SetBool(GroundedAcHash, true);
                }
                else
                {
                    AC.SetBool(GroundedAcHash, false);
                }
            }
        }
        
        public bool isFalling;
        public bool isRunning;

        public event Action OnHit;
        public event Action OnTakeItem;

        private void Start()
        {
            Run();
        }

        private float _groundedCheckDistance = 0.1f;

        private void Update()
        {
            _timeAfterLastHit += Time.deltaTime;
        }

        void FixedUpdate()
        {
            RaycastHit2D hit;
            if (hit = Physics2D.Raycast(transform.position, Vector2.down, _groundedCheckDistance, LayerMask.GetMask("Platform")))
            {
                if (!isGrounded)
                {
                    JumpCount = MaxJumpCount;
                }
                isGrounded = true;
            }
            else
            {
                isGrounded = false;
            }

            bool falling = Rigidbody.velocity.y < 0 ? true : false;
            AC.SetBool(FallingAcHash, falling);
        }

        private void OnTriggerEnter2D(Collider2D collider)
        {
            int targetLayer = collider.gameObject.layer;
            if (targetLayer == LayerMask.NameToLayer("Obstacle"))
            {
                Hit(collider.gameObject.GetComponent<Obstacle>());
            } else if (targetLayer == LayerMask.NameToLayer("Item"))
            {
                TakeItem(collider.gameObject.GetComponent<Item>());
            }
        }
        
        public void Run()
        {
            isRunning = true;
            AC.SetBool(RunningAcHash, true);
        }

        public void Stop()
        {
            isRunning = false;
            AC.SetBool(RunningAcHash, false);
        }

        public float UndamagableTimeAfterHit;
        private float _timeAfterLastHit;
        public void Hit(Obstacle obstacle)
        {
            if (!isAlive) return;
            if (UndamagableTimeAfterHit >= _timeAfterLastHit) return;
            
            print($"Got Hit By {obstacle.gameObject.name}");

            AC.SetTrigger(HitAcHash);
            GetDamage(obstacle.Damage);
            _timeAfterLastHit = 0;
            OnHit?.Invoke();
        }

        public void TakeItem(Item item)
        {
            Score += item.Score;
            item.Taken();
            print($"Got Item {item.gameObject.name} with {item.Score} score : Current Score {Score}");
            OnTakeItem?.Invoke();
        }

        public void GetDamage(int damage)
        {
            if (!isAlive) return;
            
            CurrentHP -= damage;
            print($"Got Damage {damage} : Current HP {CurrentHP}");

            if (CurrentHP <= 0)
            {
                Die();
            }
        }

        public void Die()
        {
            print("You died");
            isAlive = false;
            AC.SetBool(DieAcHash, !isAlive);
        }
    }
}
