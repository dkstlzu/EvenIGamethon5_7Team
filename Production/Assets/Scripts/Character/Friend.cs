using System;
using System.Collections.Generic;
using dkstlzu.Utility;
using MoonBunny.Dev;
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
        public static int MaxHp = 3;

        [SerializeField] private FriendSpec _spec;
        public FriendSpec Spec => _spec;
        
        [SerializeField] private FriendName _name;

        public int HorizontalSpeed;
        public float JumpPower;
        public float MagneticPower;

        
        public FriendName Name
        {
            get => _name;
            set => _name = value;
        }

        public void SetBySpec(FriendSpec spec = null)
        {
            if (spec != null)
            {
                _spec = spec;
            }

            HorizontalSpeed = _spec.HorizontalJumpSpeed;
            JumpPower = _spec.VerticalJumpSpeed;
            MagneticPower = _spec.MagneticPower;
        }
        

        public void Collect()
        {
            FriendCollectionManager.instance.Collect(Name);
        }
    }
}