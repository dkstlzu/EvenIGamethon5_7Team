using System;
using UnityEngine;

namespace MoonBunny
{
    public enum FriendName
    {
        None = -1,
        Sugar = 0,
        Sprout,
        TingkerBell,
        SodaGirl,
        Lala,
        Space,
        BlackSugar,
    }
    
    [Serializable]
    public class Friend
    {
        public static int MaxHp = 3;

        [SerializeField] private FriendSpec _spec;
        public FriendSpec Spec => _spec;
        
        public int HorizontalSpeed;
        public float JumpPower;
        public float MagneticPower;

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
    }
}