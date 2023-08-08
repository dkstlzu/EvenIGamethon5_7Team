using System;
using System.Collections.Generic;
using dkstlzu.Utility;
using UnityEngine;

namespace MoonBunny
{
    public class FriendCollectable : Gimmick
    {
        public static AudioClip S_FriendCollectedAudioClip;

        [SerializeField] private SpriteRenderer _renderer;
        public FriendName Name;

        public EnumDict<FriendName, float> Potentials;

        protected override void Reset()
        {
            base.Reset();

            Potentials.Clear();

            foreach (var friendName in (FriendName[])Enum.GetValues(typeof(FriendName)))
            {
                if (friendName == FriendName.Sugar) continue;
                if ((int)friendName < 0) continue;

                Potentials.Add(friendName, 0);
            }
        }

        private void Start()
        {
            int targetIndex = Stage.S_StageLevel;

            Name = (FriendName)(targetIndex+1);
            _renderer.sprite = PreloadedResources.instance.MemorySpriteList[targetIndex];
        }

        public override bool Invoke(MoonBunnyRigidbody with, MoonBunnyCollider.Direction direction)
        {
            if (!base.Invoke(with, direction)) return false;
            
            SoundManager.instance.PlayClip(S_FriendCollectedAudioClip);
            GameManager.instance.Stage.CollectDict[Name]++;
            Destroy(gameObject);
            return true;
        }
    }
}