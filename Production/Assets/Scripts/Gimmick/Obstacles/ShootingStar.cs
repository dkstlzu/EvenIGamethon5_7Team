using System;
using System.Collections.Generic;
using dkstlzu.Utility;
using MoonBunny.Effects;
using UnityEngine;
using Random = UnityEngine.Random;

namespace MoonBunny
{
    public class ShootingStar : Obstacle
    {
        [SerializeField] private MoonBunnyRigidbody _rigidbody;
        [SerializeField] private Sprite _invokeSprite;
        [SerializeField] private Sprite _cloudSprite;
        public float TrailCloudInterval;

        private float _timer;
        private float _lastInterval = 30;

        private void Start()
        {
#if UNITY_EDITOR
            if (!UnityEditor.EditorApplication.isPlaying) return;
#endif
            UpdateManager.instance.Delay((time) =>
            {
                _timer = time;

                if (_timer < _lastInterval - TrailCloudInterval)
                {
                    _lastInterval -= TrailCloudInterval;

                    new SpriteEffect(_cloudSprite, transform.position, 1, 3, 1).Effect();
                }
            }, null, _lastInterval);

            _lastInterval += Random.Range(-1f, 1f);
        }

        protected override void Update()
        {
            base.Update();
#if UNITY_EDITOR
            if (!UnityEditor.EditorApplication.isPlaying) return;
#endif
            _renderer.transform.Rotate(Vector3.forward, 1);
        }

        public override bool Invoke(MoonBunnyRigidbody with, MoonBunnyCollider.Direction direction)
        {
            if (!base.Invoke(with, direction)) return false;
            
            Character target = with.GetComponent<Character>();
            new InvincibleEffect(with, LayerMask.GetMask("Obstacle"), target.Renderer, target.InvincibleDuration, target.InvincibleEffectCurve).Effect();
            new SpriteEffect(_invokeSprite, transform.position, 2, 3, 1).Effect();
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