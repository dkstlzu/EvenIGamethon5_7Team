using UnityEngine;

namespace MoonBunny
{
    public class Rocket : Item
    {
        public int Duration;

        public override void Invoke(MoonBunnyRigidbody with)
        {
            base.Invoke(with);
        }
    }
}