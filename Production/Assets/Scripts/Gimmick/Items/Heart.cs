using System;
using MoonBunny.Effects;

namespace MoonBunny
{
    public class Heart : Item
    {
        public static event Action OnHeartItemTaken;
        public override bool Invoke(MoonBunnyRigidbody with, MoonBunnyCollider.Direction direction)
        {
            if (!base.Invoke(with, direction)) return false;

            new HeartEffect(with.GetComponent<Character>()).Effect();
            OnHeartItemTaken?.Invoke();
            return true;
        }
    }
}