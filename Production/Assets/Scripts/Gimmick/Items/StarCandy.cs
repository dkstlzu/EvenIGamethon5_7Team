using MoonBunny.Effects;
using UnityEngine;

namespace MoonBunny
{
    public class StarCandy : Item
    {
        public LayerMask TargetLayerMask;
        public Collider2D TargetArea;
            
        public override void Invoke(MoonBunnyRigidbody with)
        {
            base.Invoke(with);

            Vector2 _2dPosition = transform.position;

            new StarCandyEffect(TargetLayerMask, new Rect(_2dPosition + TargetArea.offset, TargetArea.bounds.size)).Effect();
        }
    }
}