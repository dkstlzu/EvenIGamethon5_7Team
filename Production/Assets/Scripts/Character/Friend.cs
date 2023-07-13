using System;
using System.Collections.Generic;
using dkstlzu.Utility;
using UnityEditor;
using UnityEngine;

namespace MoonBunny
{
    public enum FriendName
    {
        None = -1,
        First = 0,
        Second,
        Third,
    }
    
    [Serializable]
    public class Friend
    {
        [SerializeField] private FriendName _name;

        public Vector2Int JumpPower;

        public static int MaxHp = 3;
        public int CurrentHp;
        
        [SerializeField] private SpriteRenderer _renderer;
        public FriendName Name
        {
            get => _name;
            set
            {
                _name = value;
                _renderer.sprite = PreloadedResources.instance.FriendSpriteList[(int)value];

                FriendSpec spec = PreloadedResources.instance.FriendSpecList[(int)value];
                JumpPower.x = spec.HorizontalJumpSpeed;
                JumpPower.y = spec.VerticalJumpSpeed;
            }
        }

        public void Collect()
        {
            FriendCollectionManager.instance.Collect(Name);
        }
    }
}