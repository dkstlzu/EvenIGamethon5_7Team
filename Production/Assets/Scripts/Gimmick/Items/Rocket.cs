using System;
using dkstlzu.Utility;
using MoonBunny.Effects;
using UnityEngine;

namespace MoonBunny
{
    public class Rocket : Item
    {
        public static event Action<float, float> OnRocketItemTaken;
        
        public float UpSpeed;
        public float Duration;

        public override void Invoke(MoonBunnyRigidbody with)
        {
            base.Invoke(with);

            new RocketEffect(with, UpSpeed, Duration).Effect();
            OnRocketItemTaken?.Invoke(UpSpeed, Duration);
        }
    }
}