using UnityEngine;

namespace MoonBunny
{
    [CreateAssetMenu(menuName = "Specs/Obstacle")]
    public class ObstacleSpec : ScriptableObject
    {
        public int Damage;
        public bool isBouncy;
        public AudioClip AudioClip;
        public Sprite Sprite;
    }
}