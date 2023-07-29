using UnityEngine;

namespace MoonBunny
{
    public class Coin : Item
    {
        public override bool Invoke(MoonBunnyRigidbody with, MoonBunnyCollider.Direction direction)
        {
            if (!base.Invoke(with, direction)) return false;

            GameManager.instance.Stage.GoldNumber += 1;
            return true;
        }
    }
}