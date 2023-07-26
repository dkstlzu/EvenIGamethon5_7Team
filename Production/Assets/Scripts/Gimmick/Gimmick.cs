using UnityEngine;

namespace MoonBunny
{
    public class Gimmick : GridObject
    {
        public bool InvokeOnCollision = true;

        public virtual bool Invoke(MoonBunnyRigidbody with)
        {
            return true;
        }
    }
}