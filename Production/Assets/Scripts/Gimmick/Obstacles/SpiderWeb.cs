using System;
using MoonBunny.Effects;
using MoonBunny.UIs;
using UnityEngine;

namespace MoonBunny
{
    public class SpiderWeb : Obstacle
    {
        public static event Action<float, float> OnSpiderwebObstacleTaken;
            
        [Range(0, 1)] public float Slow;
        public float Duration;
        
        public override void Invoke(MoonBunnyRigidbody with)
        {
            base.Invoke(with);

            new SlowEffect(with, Slow, Duration).Effect();
            OnSpiderwebObstacleTaken?.Invoke(Slow, Duration);
            
            Destroy(gameObject);
        }
    }
}