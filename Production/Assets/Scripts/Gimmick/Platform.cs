using System.Collections.Generic;
using UnityEngine;

namespace MoonBunny
{
    public class Platform : Gimmick
    {
        public override Collision[] Collide(MoonBunnyRigidbody rigidbody, MoonBunnyCollider.Direction direction)
        {
            
            List<Collision> collisions = new List<Collision>();
            
            collisions.Add(new BlockCollision(rigidbody, this));
            
            collisions.AddRange(base.Collide(rigidbody, direction));

            return collisions.ToArray();
        }
    }
}