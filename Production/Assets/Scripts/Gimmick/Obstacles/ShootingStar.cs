using System.Collections.Generic;
using MoonBunny.Effects;
using UnityEngine;

namespace MoonBunny
{
    public class ShootingStar : Obstacle
    {
        [SerializeField] private MoonBunnyRigidbody _rigidbody;

        public override bool Invoke(MoonBunnyRigidbody with, MoonBunnyCollider.Direction direction)
        {
            if (!base.Invoke(with, direction)) return false;
            
            Character target = with.GetComponent<Character>();
            new InvincibleEffect(with, LayerMask.GetMask("Obstacle"), target.Renderer, target.InvincibleDuration, target.InvincibleEffectCurve).Effect();
            return true;
        }

        public override Collision[] Collide(MoonBunnyRigidbody rigidbody, MoonBunnyCollider.Direction direction)
        {
            List<Collision> collisions = new List<Collision>();

            if (rigidbody.GridObject is Character character)
            {
                collisions.Add(new BounceCollision(rigidbody, this));
            }
            
            collisions.AddRange(base.Collide(rigidbody, direction));

            return collisions.ToArray();
        }
    }
}