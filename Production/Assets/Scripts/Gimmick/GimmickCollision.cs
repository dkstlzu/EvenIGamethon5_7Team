using MoonBunny;
using UnityEngine;

public class Collision
{
    public GridObject Other;

    public Collision(GridObject other)
    {
        Other = other;
    }
        
    public virtual void OnCollision()
    {
        
    }
}

public class GimmickCollision : Collision
{
    public Gimmick Gimmick => (Gimmick)Other;
    public GimmickCollision(Gimmick gimmick) : base(gimmick)
    {
    }
}

public class ItemCollision : GimmickCollision
{
    public Item Item => (Item)Other;
    public ItemCollision(Item item) : base(item)
    {
    }
}

public class ObstacleCollision : GimmickCollision
{
    public Obstacle Obstacle => (Obstacle)Other;
    public ObstacleCollision(Obstacle obstacle) : base(obstacle)
    {
    }
}

public class BouncyPlatformCollision : GimmickCollision
{
    public BouncyPlatform Platform => (BouncyPlatform)Other;
    public BouncyPlatformCollision(BouncyPlatform bouncyPlatform) : base(bouncyPlatform)
    {
    }

    public override void OnCollision()
    {
        base.OnCollision();
    }
}
