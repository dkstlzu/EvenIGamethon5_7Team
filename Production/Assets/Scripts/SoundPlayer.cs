using System;
using UnityEngine;

namespace MoonBunny
{
    public class SoundPlayer : MonoBehaviour
    {
        public AudioClip AudioClip;

        private void Start()
        {
            SoundManager.instance.PlayClip(AudioClip);
        }
    }
}