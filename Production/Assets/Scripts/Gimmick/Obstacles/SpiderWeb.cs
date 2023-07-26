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
        
        public override bool Invoke(MoonBunnyRigidbody with)
        {
            if (!base.Invoke(with)) return false;

            float targetSlow = Slow;

            Character character;
            if (with.TryGetComponent(out character))
            {
                if (character.Friend.Name == FriendName.Sprout) targetSlow /= 2;
            }
            
            new SlowEffect(with, targetSlow, Duration).Effect();
            OnSpiderwebObstacleTaken?.Invoke(Slow, Duration);
            
            Destroy(gameObject);
            return true;
        }
    }
}