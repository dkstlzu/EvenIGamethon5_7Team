using UnityEngine;

namespace EvenI7.ProtoScreenSplit
{
    [CreateAssetMenu(menuName = "Scriptable/Friend")]
    public class FriendCharacterStats : ScriptableObject
    {
        public float HorizontalSpeed;
        public float JumpPower;
        public float PushingPlatformPower;
        public int MaxHp;
    }
}