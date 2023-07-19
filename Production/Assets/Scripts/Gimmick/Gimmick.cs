using UnityEngine;

namespace MoonBunny
{
    public class Gimmick : GridObject
    {
        public bool InvokeOnCollision = true;
        public virtual void Invoke(MoonBunnyRigidbody with)
        {
        }
    }
}