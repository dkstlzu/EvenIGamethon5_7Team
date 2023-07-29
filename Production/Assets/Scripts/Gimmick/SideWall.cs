using System;
using UnityEngine;

namespace MoonBunny
{
    public class SideWall : FieldObject, ICollidable
    {
        public Collision[] Collide(MoonBunnyRigidbody rigidbody, MoonBunnyCollider.Direction direction)
        {
            if (rigidbody.GridObject is Character || rigidbody.GridObject is Bee)
            {
                return new Collision[] { new FlipCollision(false, rigidbody, this) };
            } else if (rigidbody.GridObject is Crow)
            {
                return new Collision[] { new BlockCollision(rigidbody, this) };
            }

            return Array.Empty<Collision>();
        }
    }
}