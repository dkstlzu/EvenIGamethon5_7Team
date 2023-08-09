using System;
using dkstlzu.Utility;
using MoonBunny.Effects;
using UnityEngine;

namespace MoonBunny
{
    public class Crow : Obstacle
    {
        public float FlySpeed;
        public float CoolTime;
        public Color CoolTimeColor;
        private bool _enabled = true;
        [SerializeField] private MoonBunnyRigidbody _rigidbody;
        [SerializeField] private Transform _pickingPoint;
        [SerializeField] private Animator _animator;
        private TransformForceEffect _picker;
        private MoonBunnyRigidbody _pickingRigidbody;

        [SerializeField] private GimmickInvoker _invoker;

        private void Start()
        {
#if UNITY_EDITOR
            if (!UnityEditor.EditorApplication.isPlaying) return;
#endif
            
            _rigidbody.OnAfterEnterCollision += OnAfterSideWallCollision;
        }

        private void OnAfterSideWallCollision(Collision sideWallCollision)
        {
            if (sideWallCollision.Other is SideWall)
            {
                if (_picker != null)
                {
                    _pickingRigidbody.ForcePosition(_pickingRigidbody.transform.position);
                    _pickingRigidbody.StopMove();
                    _pickingRigidbody.UnpauseMove();
                    Character character = _pickingRigidbody.GetComponent<Character>();
                    new InvincibleEffect(_pickingRigidbody, LayerMask.GetMask("Obstacle"), character.Renderer, character.InvincibleDuration,
                        character.InvincibleEffectCurve).Effect();
                
                    _picker.Quit();
                    _picker = null;
                    _pickingRigidbody = null;
                    
                    _enabled = false;
                    _renderer.color = CoolTimeColor;
            
                    UpdateManager.instance.Delay(() =>
                    {
                        _enabled = true;
                        _renderer.color = Color.white;
                        _invoker.InvokeOnCollision = true;
                        InvokeOnCollision = true;
                    }, CoolTime);
                }
                else
                {
                    _invoker.InvokeOnCollision = true;
                    InvokeOnCollision = true;
                }

                _rigidbody.StopMove();
                _renderer.flipX = !_renderer.flipX;
                _animator.SetBool("Attack", false);
            }
        }

        public override bool Invoke(MoonBunnyRigidbody with, MoonBunnyCollider.Direction direction)
        {
            if (!_enabled) return false;
            
            if (!base.Invoke(with, direction)) return false;

            if (Character.instance.CurrentHp <= 0) return true;

            InvokeOnCollision = false;
            with.PauseMove();
            _picker = new TransformForceEffect(with.transform, _pickingPoint);
            _picker.Effect();
            _pickingRigidbody = with;
            return true;
        }

        public void Fly()
        {
            _invoker.InvokeOnCollision = false;
            
            if (GridTransform.GridPosition.x < 0)
            {
                _rigidbody.Move(new Vector2(FlySpeed, 0));
                _renderer.flipX = true;
            } else if (GridTransform.GridPosition.x > 0)
            {
                _rigidbody.Move(new Vector2(-FlySpeed, 0));
                _renderer.flipX = false;
            }
            
            _animator.SetBool("Attack", true);
        }
    }
}