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

        public List<Sprite> MemorySpriteList;
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
            List<float> potentialList = new List<float>();
            float total = 0;

            foreach (var pair in Potentials)
            {
                potentialList.Add(pair.Value);
                total += pair.Value;
            }

            float randomValue = UnityEngine.Random.Range(0f, total);

            float potentialCheck = 0;
            int targetIndex = -1;

            for (int i = 0; i < potentialList.Count; i++)
            {
                if (randomValue >= potentialCheck && randomValue < potentialCheck + potentialList[i])
                {
                    targetIndex = i;
                    break;
                }

                potentialCheck += potentialList[i];
            }

            if (targetIndex < 0)
            {
                targetIndex = potentialList.Count - 1;
            }

            Name = (FriendName)targetIndex;
            _renderer.sprite = MemorySpriteList[targetIndex];
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