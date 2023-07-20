using System;
using System.Collections;
using dkstlzu.Utility;
using UnityEngine;

namespace MoonBunny.Effects
{
    public interface IEffect
    {
        void Effect();
    }

    public class MagnetEffect : IEffect
    {
        private CircleCollider2D _target;
        private SpriteRenderer _spriteRenderer;
        private float _magneticPower;
        private float _duration = -1;

        public MagnetEffect(CircleCollider2D target, float magneticPower)
        {
            _target = target;
            _spriteRenderer = target.GetComponentInChildren<SpriteRenderer>();
            _magneticPower = magneticPower;
        }
        
        public MagnetEffect(CircleCollider2D target, float magneticPower, float duration) : this(target, magneticPower)
        {
            _duration = duration;
        }

        public void Effect()
        {
            float previousPower = _target.radius;
            
            _target.radius += _magneticPower;
            _spriteRenderer.transform.localScale = new Vector3(_magneticPower * 2, _magneticPower * 2, 1);

            if (_duration > 0)
            {
                CoroutineHelper.Delay(() =>
                {
                    _target.radius = previousPower;
                    _spriteRenderer.transform.localScale = new Vector3(previousPower * 2, previousPower * 2, 1);
                }, _duration);
            }
        }
    }

    public class HeartEffect : IEffect
    {
        private Character _target;

        public HeartEffect(Character target)
        {
            _target = target;
        }

        public void Effect()
        {
            _target.GetHeart();
        }
    }

    public class RocketEffect : IEffect
    {
        private MoonBunnyRigidbody _rigidbody;
        private float _upSpeed;
        private float _duration;
        
        public RocketEffect(MoonBunnyRigidbody rigidbody, float upSpeed, float duration)
        {
            _rigidbody = rigidbody;
            _upSpeed = upSpeed;
            _duration = duration;
        }
        
        public void Effect()
        {
            Vector2 previousVelocity = _rigidbody.Velocity;
            float previousGravity = _rigidbody.Gravity;
            
            _rigidbody.DisableCollision();
            _rigidbody.Move(new Vector2(0, _upSpeed));
            _rigidbody.Gravity = 0;
            
            CoroutineHelper.Delay(() =>
            {
                _rigidbody.EnableCollision();
                _rigidbody.Move(previousVelocity);
                _rigidbody.Gravity = previousGravity;
            }, _duration);
        }
    }
    
    public class StarCandyEffect : IEffect
    {
        private static int S_MaxDestroyNumber;
        
        private LayerMask _targetLayerMask;
        private Rect _area;
        
        public StarCandyEffect(LayerMask targetLayerMask, Rect area)
        {
            _targetLayerMask = targetLayerMask;
            _area = area;
        }
        
        public void Effect()
        {
            Collider2D[] results = Physics2D.OverlapAreaAll(_area.min, _area.max, _targetLayerMask);
            
            for (int i = 0; i < results.Length; i++)
            {
                if (results[i] == null) continue;
                
                MonoBehaviour.Destroy(results[i].gameObject);
            }
        }
    }

    public class InvincibleEffect : IEffect
    {
        private MoonBunnyRigidbody _target;
        private LayerMask _from;
        private SpriteRenderer _renderer;
        private float _duration;
        private AnimationCurve _curve;

        public InvincibleEffect(MoonBunnyRigidbody rigidbody, LayerMask ignore, SpriteRenderer renderer, float duration, AnimationCurve curve)
        {
            _target = rigidbody;
            _from = ignore;
            _renderer = renderer;
            _duration = duration;
            _curve = curve;
        }

        public void Effect()
        {
            _target.StartCoroutine(Flickering());
        }

        IEnumerator Flickering()
        {
            _target.IgnoreCollision(_from);
            float timer = 0;
            
            Color color = Color.white;
            while (timer < _duration)
            {
                timer += Time.deltaTime;
                color = _renderer.color;
                _renderer.color = new Color(color.r, color.g, color.b, _curve.Evaluate(timer));
                yield return new WaitForEndOfFrame();
            }

            _renderer.color = Color.white;
            _target.IgnoreCollision(0);
        }
    }
    
    public class TransformForceEffect : IEffect, IUpdatable
    {
        public Transform Target;
        public Transform To;

        public TransformForceEffect(Transform target, Transform to)
        {
            Target = target;
            To = to;
        }

        public void Update()
        {
            Target.transform.position = To.position;
        }

        public void Effect()
        {
            UpdateManager.instance.Register(this);
        }

        public void Quit()
        {
            UpdateManager.instance.Unregister(this);
        }
    }

    public class SlowEffect : IEffect
    {
        private MoonBunnyRigidbody _target;
        private float _slow;
        private float _duration;
        
        public SlowEffect(MoonBunnyRigidbody target, float slow, float duration)
        {
            _target = target;
            _slow = slow;
            _duration = duration;
        }

        public void Effect()
        {
            _target.ChangeDelta(_slow, _duration);
        }
    }
}