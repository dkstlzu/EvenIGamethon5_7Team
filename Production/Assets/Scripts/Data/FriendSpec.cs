using UnityEngine;

namespace MoonBunny
{
    [CreateAssetMenu(menuName = "Specs/Friend")]
    public class FriendSpec : ScriptableObject
    {
        public int HorizontalJumpSpeed;
        public float VerticalJumpSpeed;
        public float MagneticPower;
        public string SpecialAbility;
    }
}