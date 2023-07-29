using System;
using MoonBunny.Effects;
using UnityEngine;

namespace MoonBunny
{
    public class StarCandy : Item
    {
        public LayerMask TargetLayerMask;
        public Collider2D TargetArea;
            
        public override bool Invoke(MoonBunnyRigidbody with, MoonBunnyCollider.Direction direction)
        {
            if (!base.Invoke(with, direction)) return false;

            Vector2 _2dPosition = transform.position;

            new StarCandyEffect(TargetLayerMask, new Bounds(_2dPosition + TargetArea.offset, TargetArea.bounds.size)).Effect();
            return true;
        }

        private void OnDrawGizmos()
        {
            Vector3 offset = TargetArea.offset;
            Vector3 center = transform.position + offset;
            Gizmos.DrawCube(center, TargetArea.bounds.size);
        }
    }
}