using System;
using MoonBunny.Effects;
using UnityEngine;

namespace MoonBunny
{
    public class Magnet : Item
    {
        public static event Action<float, float> OnMangetItemTaken;

        [RuntimeInitializeOnLoadMethod]
        static void ClearEventListeners()
        {
            OnMangetItemTaken = null;
        }
        
        public float MagnetPower;
        public float Duration;

        public override bool Invoke(MoonBunnyRigidbody with)
        {
            if (!base.Invoke(with)) return false;

            new MagnetEffect(with.GetComponentInChildren<CircleCollider2D>(), MagnetPower, Duration).Effect();
            OnMangetItemTaken?.Invoke(MagnetPower, Duration);
            return true;
        }
    }
}