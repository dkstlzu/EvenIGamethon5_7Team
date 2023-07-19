using UnityEngine;

namespace MoonBunny
{
    public class Heart : Item
    {
        public override void Invoke(MoonBunnyRigidbody with)
        {
            base.Invoke(with);
            
            with.GetComponent<Character>().GetHeart();
        }
    }
}