using UnityEngine;

namespace MoonBunny
{
    [CreateAssetMenu(menuName = "Specs/Friend")]
    public class FriendSpec : ScriptableObject
    {
        public int HorizontalJumpSpeed;
        public int VerticalJumpSpeed;
    }
}