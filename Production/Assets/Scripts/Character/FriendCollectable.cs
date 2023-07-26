using System;
using System.Collections.Generic;
using dkstlzu.Utility;
using UnityEngine;

namespace MoonBunny
{
    public class FriendCollectable : Gimmick
    {
        public static AudioClip S_FriendCollectedAudioClip;
        public FriendName Name;

        public EnumDict<FriendName, float> Potentials;

        private void Start()
        {
            List<float> potentialList = new List<float>();
            float total = 0;

            foreach (var pair in Potentials)
            {
                potentialList.Add(pair.Value);
                total += pair.Value;
            }
        }

        public override void Invoke(MoonBunnyRigidbody with)
        {
            base.Invoke(with);
            
            SoundManager.instance.PlayClip(S_FriendCollectedAudioClip);
            GameManager.instance.Stage.CollectDict[Name]++;
            Destroy(gameObject);
        }
    }
}