using System;
using MoonBunny.Effects;
using UnityEngine;

namespace MoonBunny
{
    public class Magnet : Item
    {
        public static event Action<float, float> OnMangetItemTaken;
        
        public float MagnetPower;
        public float Duration;

        public override void Invoke(MoonBunnyRigidbody with)
        {
            base.Invoke(with);
            
            new MagnetEffect(with.GetComponentInChildren<CircleCollider2D>(), MagnetPower, Duration).Effect();
            OnMangetItemTaken?.Invoke(MagnetPower, Duration);

        }
    }
}