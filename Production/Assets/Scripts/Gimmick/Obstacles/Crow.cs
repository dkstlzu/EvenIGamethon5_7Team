using System;
using UnityEngine;

namespace MoonBunny
{
    public class Crow : Obstacle
    {
        public float FlySpeed;
        [SerializeField] private MoonBunnyRigidbody _rigidbody;
        [SerializeField] private Transform _pickingPoint;
        [SerializeField] private Animator _animator;
        private int _flyingDirection = 0;
        private TransformForce _picker;
        private MoonBunnyRigidbody _pickingRigidbody;

        private void Start()
        {
#if UNITY_EDITOR
            if (!UnityEditor.EditorApplication.isPlaying) return;
#endif
            
            _rigidbody.OnEnterCollision += OnSideWallCollision;
        }

        private void OnSideWallCollision(Collision sideWallCollision)
        {
            if (sideWallCollision.Other is SideWall)
            {
                if (_picker != null)
                {
                    _pickingRigidbody.ForcePosition(_pickingRigidbody.transform.position);
                    _pickingRigidbody.StopMove();
                    _pickingRigidbody.UnpauseMove();
                
                    _picker = null;
                    _pickingRigidbody = null;
                }

                _rigidbody.StopMove();
                _renderer.flipX = !_renderer.flipX;
                _animator.SetBool("Attack", false);
            }
        }

        public override void Invoke(MoonBunnyRigidbody with)
        {
            base.Invoke(with);

            with.PauseMove();
            _picker = new TransformForce(with.transform, _pickingPoint);
            _pickingRigidbody = with;
        }

        protected override void Update()
        {
            base.Update();

#if UNITY_EDITOR
            if (!UnityEditor.EditorApplication.isPlaying) return;
#endif

            if (_picker != null)
            {
                _picker.UpdateForce();
            }
        }

        public void Fly()
        {
            if (GridTransform.GridPosition.x < 0)
            {
                _rigidbody.Move(new Vector2(FlySpeed, 0));
                _renderer.flipX = true;
                _flyingDirection = 1;
            } else if (GridTransform.GridPosition.x > 0)
            {
                _rigidbody.Move(new Vector2(-FlySpeed, 0));
                _renderer.flipX = false;
                _flyingDirection = -1;
            }
            
            _animator.SetBool("Attack", true);
        }
    }
}