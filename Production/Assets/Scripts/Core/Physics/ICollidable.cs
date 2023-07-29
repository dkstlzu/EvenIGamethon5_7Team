namespace MoonBunny
{
    public interface ICollidable
    {
        Collision[] Collide(MoonBunnyRigidbody rigidbody, MoonBunnyCollider.Direction direction);
    }
}