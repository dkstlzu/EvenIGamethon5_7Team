using UnityEngine;

namespace MoonBunny
{
    [CreateAssetMenu(fileName = "BouncyPlatformSetting", menuName = "Specs/BouncyPlatformSetting", order = 0)]
    public class BouncyPlatformSetting : ScriptableObject
    {
        public float Bounciness;
    }
}