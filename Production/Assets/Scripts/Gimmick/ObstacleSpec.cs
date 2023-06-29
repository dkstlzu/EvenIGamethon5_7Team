using UnityEngine;

namespace MoonBunny
{
    [CreateAssetMenu(fileName = "_Obstacle", menuName = "Specs/Obstacle", order = 0)]
    public class ObstacleSpec : ScriptableObject
    {
        public int Damage;
        public bool isBouncy;
    }
}