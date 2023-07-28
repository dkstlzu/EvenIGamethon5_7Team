using MoonBunny.Effects;
using UnityEngine;

namespace MoonBunny
{
    public class PinWheel : Obstacle
    {
        public override bool Invoke(MoonBunnyRigidbody with)
        {
            if (!base.Invoke(with))
            {
                new StarCandyEffect(LayerMask.GetMask("Obstacle"), new Bounds(transform.position, new Vector2(50, 3 * GridTransform.GridSetting.GridHeight))).Effect();
                return false;
            }
            
            Character target = with.GetComponent<Character>();

            new InvincibleEffect(with, LayerMask.GetMask("Obstacle"), target.Renderer, target.InvincibleDuration, target.InvincibleEffectCurve).Effect();

            return true;
        }
    }
}