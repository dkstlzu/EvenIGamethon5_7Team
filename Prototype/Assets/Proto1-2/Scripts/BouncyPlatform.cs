using System;
using UnityEngine;

namespace EvenI7.Proto1_2
{
    class BouncyPlatform : Platform
    {
        public Animator AC;
        public float JumpPower;

        private readonly int _jumpAcHash = Animator.StringToHash("Jump");

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.tag == "Player")
            {
                var character = other.gameObject.GetComponent<Proto1_2Character>();
                if (character)
                    character.BouncyPlatformJump(JumpPower);
                AC.SetTrigger(_jumpAcHash);
            }
        }
    }
}