using System;
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

        public override void Invoke(MoonBunnyRigidbody with)
        {
            base.Invoke(with);
            
            Character target = with.GetComponent<Character>();
            new InvincibleEffect(with, LayerMask.GetMask("Obstacle"), target.Renderer, target.InvincibleDuration, target.InvincibleEffectCurve).Effect();
        }
    }
}