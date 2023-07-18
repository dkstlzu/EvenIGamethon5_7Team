using UnityEngine;

namespace MoonBunny
{
    [CreateAssetMenu(menuName = "Specs/Friend")]
    public class FriendSpec : ScriptableObject
    {
        public FriendName Name;
        public int HorizontalJumpSpeed;
        public int VerticalJumpSpeed;
        public int MagneticPower;
    }
}