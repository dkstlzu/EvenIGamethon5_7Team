using UnityEngine;

namespace MoonBunny
{
    [CreateAssetMenu(fileName = "StageSpec", menuName = "Specs/StageSpec", order = 0)]
    public class StageSpec : ScriptableObject
    {
        public int Height;
        public int FirstStepScore;
        public int SecondStepScore;
        public int ThirdStepScore;

        [Header("Random Items")]
        public int RicecakeNumber;
        [Range(0, 1)] public float RainbowRicecakeRatio;
        public int CoinNumber;
        public int FriendCollectableNumber;

        [Header("Thunder")]
        public bool SummonThunderEnabled;
        public float SummonThunderInterval;
        public float ThunderWarningTime;
            
        [Header("ShootingStar")]
        public bool SummonShootingStarEnabled;
        public float SummonShootingStarInterval;
        public float ShootingStarWarningTime;
    }
}