using MoonBunny.Effects;
using UnityEngine;

namespace MoonBunny
{
    public class PinWheel : Obstacle
    {
        public override void Invoke(MoonBunnyRigidbody with)
        {
            base.Invoke(with);
            
            Character target = with.GetComponent<Character>();
            new InvincibleEffect(with, LayerMask.GetMask("Obstacle"), target.Renderer, target.InvincibleDuration, target.InvincibleEffectCurve).Effect();
        }
    }
}