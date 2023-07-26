using UnityEngine;

namespace MoonBunny
{
    public class Coin : Item
    {
        public override bool Invoke(MoonBunnyRigidbody with)
        {
            if (!base.Invoke(with)) return false;

            GameManager.instance.GoldNumber += 1;
            return true;
        }
    }
}