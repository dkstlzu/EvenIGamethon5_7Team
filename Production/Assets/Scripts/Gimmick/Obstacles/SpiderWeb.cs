using System;
using MoonBunny.Effects;
using MoonBunny.UIs;
using UnityEngine;

namespace MoonBunny
{
    public class SpiderWeb : Obstacle
    {
        public static event Action<float, float> OnSpiderwebObstacleTaken;
        
        [RuntimeInitializeOnLoadMethod]
        static void ClearEventListeners()
        {
            OnSpiderwebObstacleTaken = null;
        }
            
        [Range(0, 1)] public float Slow;
        public float Duration;
        
        public override bool Invoke(MoonBunnyRigidbody with, MoonBunnyCollider.Direction direction)
        {
            if (!base.Invoke(with, direction)) return false;

            float targetSlow = Slow;

            Character character;
            if (with.TryGetComponent(out character))
            {
                if (character.Friend.Name == FriendName.Sprout)
                {
                    targetSlow = 1 - ((1 - targetSlow) / 2);
                } else if (character.Friend.Name == FriendName.Soda)
                {
                    
                }
            }
            
            new SlowEffect(with, targetSlow, Duration).Effect();
            OnSpiderwebObstacleTaken?.Invoke(Slow, Duration);
            
            Destroy(gameObject);
            return true;
        }

        public override Collision[] Collide(MoonBunnyRigidbody rigidbody, MoonBunnyCollider.Direction direction)
        {
            if (rigidbody.GridObject is Character character)
            {
                if (character.Friend.Name == FriendName.Soda)
                {
                    return new Collision[]{new DestroyCollision(rigidbody, this)};
                }
            }
            
            return base.Collide(rigidbody, direction);
        }
    }
}