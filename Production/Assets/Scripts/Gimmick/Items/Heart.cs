using System;
using MoonBunny.Effects;

namespace MoonBunny
{
    public class Heart : Item
    {
        public static event Action OnHeartItemTaken;
        public override bool Invoke(MoonBunnyRigidbody with)
        {
            if (!base.Invoke(with)) return false;

            new HeartEffect(with.GetComponent<Character>()).Effect();
            OnHeartItemTaken?.Invoke();
            return true;
        }
    }
}