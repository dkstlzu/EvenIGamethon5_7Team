using System;
using System.Collections.Generic;
using MoonBunny.Dev;
using UnityEngine;

namespace MoonBunny
{
    public class GameOverFloor : Platform
    {
        public override bool Invoke(MoonBunnyRigidbody with, MoonBunnyCollider.Direction direction)
        {
            if (!base.Invoke(with, direction)) return false;

            Character character = with.GetComponent<Character>();
            if (character.FirstJumped)
            {
                character.Hit(null);
                character.Hit(null);
                character.Hit(null);
                GameManager.instance.Stage.Fail();
                return true;
            }

            return false;
        }

        public override Collision[] Collide(MoonBunnyRigidbody rigidbody, MoonBunnyCollider.Direction direction)
        {
            List<Collision> collisions = new List<Collision>();

            if (rigidbody.GridObject is Gimmick gimmick)
            {
                collisions.Add(new DestroyCollision(rigidbody, gimmick));
            }
            
            collisions.AddRange(base.Collide(rigidbody, direction));

            return collisions.ToArray();
        }
    }
}