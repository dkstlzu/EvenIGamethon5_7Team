using System;
using UnityEngine;

namespace EvenI7.ProtoScreenSplit
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Bouncable : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D _rigidbody2D;

        private void Reset()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
        }

        private Vector2 _lastVelocity;
        private void FixedUpdate()
        {
            _lastVelocity = _rigidbody2D.velocity;
        }

        public void Bounce()
        {
            _rigidbody2D.velocity = -_lastVelocity;
        }

        public void BounceX()
        {
            _rigidbody2D.velocity = new Vector2(-_lastVelocity.x, _lastVelocity.y);
        }

        public void BounceY()
        {
            _rigidbody2D.velocity = new Vector2(_lastVelocity.x, -_lastVelocity.y);
        }

        public void BounceUp(float boundPower)
        {
            float targetYPower = Mathf.Max(boundPower, -_lastVelocity.y);
            _rigidbody2D.velocity = new Vector2(_lastVelocity.x, targetYPower);

        }
    }
}