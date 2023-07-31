﻿using System;
using dkstlzu.Utility;
using MoonBunny.Effects;
using UnityEngine;

namespace MoonBunny
{
    public class Rocket : Item
    {
        public static event Action<float, float> OnRocketItemTaken;
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void ClearEventListeners()
        {
            OnRocketItemTaken = null;
        }
        
        public float UpSpeed;
        public float Duration;

        public override bool Invoke(MoonBunnyRigidbody with, MoonBunnyCollider.Direction direction)
        {
            if (!base.Invoke(with, direction)) return false;

            new RocketEffect(with, UpSpeed, Duration).Effect();
            OnRocketItemTaken?.Invoke(UpSpeed, Duration);
            return true;
        }
    }
}