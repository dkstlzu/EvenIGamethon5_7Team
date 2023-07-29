using System.Collections.Generic;
using UnityEngine;

namespace MoonBunny
{
    public class Gimmick : GridObject, ICollidable
    {
        public bool InvokeOnCollision = true;

        public virtual bool Invoke(MoonBunnyRigidbody with, MoonBunnyCollider.Direction direction)
        {
            return true;
        }


        public virtual Collision[] Collide(MoonBunnyRigidbody rigidbody, MoonBunnyCollider.Direction direction)
        {
            List<Collision> collisions = new List<Collision>();
            
            if (rigidbody.GridObject is Character)
            {
                collisions.Add(new GimmickCollision(rigidbody, direction, this));
            }

            return collisions.ToArray();
        }
    }
}