using UnityEngine;

namespace MoonBunny
{
    [CreateAssetMenu(menuName = "Specs/Friend")]
    public class FriendSpec : ScriptableObject
    {
        public float StartJumpHorizontalSpeed;
        public float StartJumpVerticalSpeed;
        public float BouncyPower;
        public float PushingPlatformPower;
        public int MaxHp;
    }
}