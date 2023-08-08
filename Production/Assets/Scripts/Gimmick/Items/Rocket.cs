using System;
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
        public float UpGridHeight;

        public override bool Invoke(MoonBunnyRigidbody with, MoonBunnyCollider.Direction direction)
        {
            if (!base.Invoke(with, direction)) return false;

            float duration = UpGridHeight * GridTransform.GridSetting.GridHeight / UpSpeed;
            new RocketEffect(with, UpSpeed, duration).Effect();
            OnRocketItemTaken?.Invoke(UpSpeed, duration);
            return true;
        }
    }
}