using System;
using System.Collections.Generic;
using MoonBunny.Effects;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace MoonBunny
{
    public class Bee : Obstacle
    {
        public float FlySpeed;
        [SerializeField] private MoonBunnyRigidbody _rigidbody;

        private void Start()
        {
#if UNITY_EDITOR
            if (!UnityEditor.EditorApplication.isPlaying) return;
#endif
            _rigidbody.Move(new Vector2(FlySpeed, 0));
        }

        protected override void Update()
        {
            base.Update();
#if UNITY_EDITOR
            if (!UnityEditor.EditorApplication.isPlaying) return;
#endif

            _renderer.flipX = _rigidbody.isMovingToLeft;
        }

        public override bool Invoke(MoonBunnyRigidbody with, MoonBunnyCollider.Direction direction)
        {
            if (!base.Invoke(with, direction)) return false;
            
            Character target = with.GetComponent<Character>();
            new InvincibleEffect(with, LayerMask.GetMask("Obstacle"), target.Renderer, target.InvincibleDuration, target.InvincibleEffectCurve)
                .Effect();
            return true;
        }
        
        public override Collision[] Collide(MoonBunnyRigidbody rigidbody, MoonBunnyCollider.Direction direction)
        {
            List<Collision> collisions = new List<Collision>();

            if (rigidbody.GridObject is Character character)
            {
                collisions.Add(new BounceCollision(rigidbody, this));
            }
            
            collisions.AddRange(base.Collide(rigidbody, direction));

            return collisions.ToArray();
        }
    }
}