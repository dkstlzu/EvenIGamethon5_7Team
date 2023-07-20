using MoonBunny.Effects;
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
            
            Character target = with.GetComponent<Character>();
            new InvincibleEffect(with, LayerMask.GetMask("Obstacle"), target.Renderer, target.InvincibleDuration, target.InvincibleEffectCurve).Effect();
        }

        public void Fall()
        {
            _rigidbody.Move(new Vector2(0, -FallingSpeed));
        }
    }
}