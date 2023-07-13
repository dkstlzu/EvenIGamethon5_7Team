using UnityEngine;

namespace MoonBunny
{
    public class Gimmick : GridObject
    {
        public virtual void Invoke()
        {
            print("Gimmick Invoke");
        }
    }
}