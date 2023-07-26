using MoonBunny.Effects;
using UnityEngine;

namespace MoonBunny
{
    public class StarCandy : Item
    {
        public LayerMask TargetLayerMask;
        public Collider2D TargetArea;
            
        public override bool Invoke(MoonBunnyRigidbody with)
        {
            if (!base.Invoke(with)) return false;

            Vector2 _2dPosition = transform.position;

            new StarCandyEffect(TargetLayerMask, new Rect(_2dPosition + TargetArea.offset, TargetArea.bounds.size)).Effect();
            return true;
        }
    }
}