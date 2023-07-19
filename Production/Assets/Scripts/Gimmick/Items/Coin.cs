using UnityEngine;

namespace MoonBunny
{
    public class Coin : Item
    {
        public override void Invoke(MoonBunnyRigidbody with)
        {
            base.Invoke(with);

            GameManager.instance.GoldNumber += 1;
        }
    }
}