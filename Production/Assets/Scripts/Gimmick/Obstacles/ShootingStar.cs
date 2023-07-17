using UnityEngine;

namespace MoonBunny
{
    public class ShootingStar : Obstacle
    {
        public float FallingSpeed;
        [SerializeField] private MoonBunnyRigidbody _rigidbody;

        public override void Invoke(MoonBunnyRigidbody with)
        {
            base.Invoke(with);
            
        }

        public void Fall()
        {
            _rigidbody.Move(new Vector2(0, -FallingSpeed));
        }
    }
}