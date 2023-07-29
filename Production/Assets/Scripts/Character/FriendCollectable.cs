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

        protected override void Reset()
        {
            base.Reset();

            foreach (var friendName in (FriendName[])Enum.GetValues(typeof(FriendName)))
            {
                Potentials.Add(friendName, 0);
            }
        }

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

        public override bool Invoke(MoonBunnyRigidbody with)
        {
            if (!base.Invoke(with)) return false;
            
            SoundManager.instance.PlayClip(S_FriendCollectedAudioClip);
            GameManager.instance.Stage.CollectDict[Name]++;
            Destroy(gameObject);
            return true;
        }
    }
}