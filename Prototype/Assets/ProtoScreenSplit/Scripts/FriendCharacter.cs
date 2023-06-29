using System;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEditor;
using UnityEngine;

namespace EvenI7.ProtoScreenSplit
{
    public enum FriendName
    {
        None = -1,
        First = 0,
        Second,
        Third,
    }
    
    [Serializable]
    public class FriendCharacter
    {
        public float HorizontalSpeed;
        public float JumpPower;
        public float PushingPlatformPower;

        public int MaxHp;
        public int CurrentHp;
        
        [SerializeField] private FriendName _name;
        public FriendName Name
        {
            get => _name;
            set
            {
                _name = value;
                renderer.sprite = SFriendSpriteList[(int)value];

                FriendCharacterStats stat = SFriendStatsList[(int)value];
                HorizontalSpeed = stat.HorizontalSpeed;
                JumpPower = stat.JumpPower;
                PushingPlatformPower = stat.PushingPlatformPower;
                MaxHp = stat.MaxHp;
            }
        }
        public static List<Sprite> SFriendSpriteList;
        public static List<FriendCharacterStats> SFriendStatsList;
        public static bool SSpriteSetUp;
        [SerializeField] private string friendCharacterSpritePath;
        [SerializeField] private SpriteRenderer renderer;


        public event Action<FriendCharacter> OnCharacterCollectFinished;

        public int TargetCollectingNumber
        {
            get => FriendCollectionManager.instance.GetTargetNumber(Name);
        }

        public int CurrentCollectingNumber
        {
            get => FriendCollectionManager.instance.GetCurrentNumber(Name);
        }

        private const int FriendNumber = 3;
        public void LoadSprites()
        {
            if (SSpriteSetUp) return;

            SFriendSpriteList = new List<Sprite>();
            SFriendStatsList = new List<FriendCharacterStats>();
                
            string[] friendSpriteNames = Enum.GetNames(typeof(FriendName));
            string spritePath = "Sprites/Characters/";

            for (int i = 0; i < FriendNumber; i++)
            {
                Sprite[] sprites  = Resources.LoadAll<Sprite>(spritePath + friendSpriteNames[i]);
                SFriendSpriteList.Add(sprites[0]);
            }

            string statPath = "Friends/";
            
            for (int i = 0; i < FriendNumber; i++)
            {
                FriendCharacterStats stats  = Resources.Load<FriendCharacterStats>(statPath + friendSpriteNames[i]);
                Debug.Log($"{stats}");
                SFriendStatsList.Add(stats);
            }

            SSpriteSetUp = true;
        }

        public void Collect()
        {
            FriendCollectionManager.instance.Collect(Name);
        }
    }
}