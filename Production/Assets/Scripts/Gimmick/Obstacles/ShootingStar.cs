using MoonBunny.Effects;
using UnityEngine;

namespace MoonBunny
{
    public class ShootingStar : Obstacle
    {
        [SerializeField] private MoonBunnyRigidbody _rigidbody;

        public override bool Invoke(MoonBunnyRigidbody with)
        {
            if (!base.Invoke(with)) return false;
            
            Character target = with.GetComponent<Character>();
            new InvincibleEffect(with, LayerMask.GetMask("Obstacle"), target.Renderer, target.InvincibleDuration, target.InvincibleEffectCurve).Effect();
            return true;
        }
    }
}