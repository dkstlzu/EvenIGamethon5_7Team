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
        public float StartJumpHorizontalSpeed;
        public float StartJumpVerticalSpeed;
        public float BouncyPower;
        public float PushingPlatformPower;

        public int MaxHp;
        public int CurrentHp;
        
        [SerializeField] private FriendName _name;
        [SerializeField] private SpriteRenderer _renderer;
        public FriendName Name
        {
            get => _name;
            set
            {
                _name = value;
                _renderer.sprite = PreloadedResources.instance.FriendSpriteList[(int)value];

                FriendSpec spec = PreloadedResources.instance.FriendSpecList[(int)value];
                StartJumpHorizontalSpeed = spec.StartJumpHorizontalSpeed;
                StartJumpVerticalSpeed = spec.StartJumpVerticalSpeed;
                BouncyPower = spec.BouncyPower;
                PushingPlatformPower = spec.PushingPlatformPower;
                MaxHp = spec.MaxHp;
            }
        }

        public void Collect()
        {
            FriendCollectionManager.instance.Collect(Name);
        }
    }
}