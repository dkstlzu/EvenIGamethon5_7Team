using System;
using MoonBunny.Effects;

namespace MoonBunny
{
    public class Heart : Item
    {
        public static event Action OnHeartItemTaken;
        public override void Invoke(MoonBunnyRigidbody with)
        {
            base.Invoke(with);
            
            new HeartEffect(with.GetComponent<Character>()).Effect();
            OnHeartItemTaken?.Invoke();
        }
    }
}