using dkstlzu.Utility;
using UnityEngine;

namespace MoonBunny
{
    public class Rocket : Item
    {
        public float UpSpeed;
        public float Duration;

        public override void Invoke(MoonBunnyRigidbody with)
        {
            base.Invoke(with);

            Vector2 previousVelocity = with.Velocity;
            float previousGravity = with.Gravity;
            
            with.DisableCollision();
            with.Move(new Vector2(0, UpSpeed));
            with.Gravity = 0;
            
            CoroutineHelper.Delay(() =>
            {
                with.EnableCollision();
                with.Move(previousVelocity);
                with.Gravity = previousGravity;
            }, Duration);

            GetComponent<Collider2D>().enabled = false;
            _renderer.enabled = false;
            Destroy(gameObject, 2);
        }
    }
}