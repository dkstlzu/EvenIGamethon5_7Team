using UnityEngine;

namespace MoonBunny
{
    public class SpiderWeb : Obstacle
    {
        [Range(0, 1)] public float Slow;
        public float Duration;
        
        public override void Invoke(MoonBunnyRigidbody with)
        {
            base.Invoke(with);

            with.ChangeDelta(with.MoveSacle * Slow, Duration);
            Destroy(gameObject);
        }
    }
}