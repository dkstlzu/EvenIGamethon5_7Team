using System;
using dkstlzu.Utility;
using UnityEngine;

namespace MoonBunny
{
    public enum FriendName
    {
        None = -1,
        [StringValue("설탕이")]
        Sugar = 0,
        [StringValue("새싹이")]
        Sprout,
        [StringValue("팅클이")]
        Tingkle,
        [StringValue("소다")]
        Soda,
        [StringValue("라라")]
        Lala,
        [StringValue("암토롱")]
        Amtorong,
        [StringValue("흑당이")]
        BlackSugar,
    }
    
    [Serializable]
    public class Friend
    {
        public static int MaxHp = 3;

        [SerializeField] private FriendSpec _spec;
        public FriendSpec Spec => _spec;
        public FriendName Name;
        
        public int HorizontalSpeed;
        public float JumpPower;
        public float MagneticPower;

        public void SetBySpec(FriendSpec spec = null)
        {
            if (spec != null)
            {
                _spec = spec;
            }

            FriendName name = StringValue.GetEnumValue<FriendName>(_spec.name);
            Name = name;
                
            HorizontalSpeed = _spec.HorizontalJumpSpeed;
            JumpPower = _spec.VerticalJumpSpeed;
            MagneticPower = _spec.MagneticPower;
        }
    }
}