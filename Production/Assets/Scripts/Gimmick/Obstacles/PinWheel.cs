using MoonBunny.Effects;
using UnityEngine;

namespace MoonBunny
{
    public class PinWheel : Obstacle
    {
        public override bool Invoke(MoonBunnyRigidbody with)
        {
            if (!base.Invoke(with)) return false;
            
            Character target = with.GetComponent<Character>();
            new InvincibleEffect(with, LayerMask.GetMask("Obstacle"), target.Renderer, target.InvincibleDuration, target.InvincibleEffectCurve).Effect();

            if (target.Friend.Name == FriendName.Lala || target.Friend.Name == FriendName.SodaGirl)
            {
                new StarCandyEffect(LayerMask.GetMask("Obstacle"), new Rect(transform.position, new Vector2(50, 3 * GridTransform.GridSetting.GridHeight))).Effect();
            }

            return true;
        }
    }
}