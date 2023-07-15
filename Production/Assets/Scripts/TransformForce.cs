using UnityEngine;

namespace MoonBunny
{
    public class TransformForce
    {
        public Transform Target;
        public Transform To;

        public TransformForce(Transform target, Transform to)
        {
            Target = target;
            To = to;
        }

        public void UpdateForce()
        {
            Target.position = To.position;
        }
    }
}