using UnityEngine;

namespace MoonBunny
{
    [CreateAssetMenu(menuName = "Specs/Obstacle")]
    public class ObstacleSpec : ScriptableObject
    {
        public AudioClip AudioClip;
        public Sprite Sprite;
    }
}