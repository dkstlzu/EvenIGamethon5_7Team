using System;
using dkstlzu.Utility;
using UnityEngine;
using Random = UnityEngine.Random;

namespace MoonBunny
{
    public class BouncyPlatform : Platform
    {
        private static readonly int JumpHash = Animator.StringToHash("Jump");

        [SerializeField] private Animator _animator;
        [Range(0, 1)] public float HorizontalDampingSpeed;
        public int JumpPower;

        public void PushedOut(Character by)
        {
            Vector2 direction = by.transform.position.x > transform.position.x ? Vector2.left : Vector2.right;
            
            PushedOut(by.Friend.PushingPlatformPower, direction);
        }

        void PushedOut(float power, Vector2 direction)
        {
        }
    }
}