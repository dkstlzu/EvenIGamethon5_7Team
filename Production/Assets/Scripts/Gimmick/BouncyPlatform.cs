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
        public int JumpPower;
        public int VerticalMoveRange;
        public int HorizontalMoveRange;
    }
}