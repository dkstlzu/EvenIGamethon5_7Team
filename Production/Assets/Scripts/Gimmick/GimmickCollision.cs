using System;
using MoonBunny;
using UnityEngine;
using Object = UnityEngine.Object;


// [Serializable]
public class Collision : IEquatable<Collision>
{
    [SerializeField] private FieldObject _other;
    public FieldObject Other => _other;

    public Collision(FieldObject other)
    {
        _other = other;
    }
        
    public virtual void OnCollision()
    {
        
    }

    public bool Equals(Collision other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return Equals(Other, other.Other);
    }

    public override bool Equals(object obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((Collision)obj);
    }

    public override int GetHashCode()
    {
        return (Other != null ? Other.GetHashCode() : 0);
    }
}

public class GridObjectCollision : Collision
{
    public GridObject GridObject => (GridObject)(Other);
    protected MoonBunnyRigidbody _rigidbody;

    public GridObjectCollision(MoonBunnyRigidbody rigidbody, GridObject gridObject) : base(gridObject)
    {
        _rigidbody = rigidbody;
    }
}

public class GimmickCollision : GridObjectCollision
{
    public Gimmick Gimmick => (Gimmick)Other;
    public GimmickCollision(MoonBunnyRigidbody rigidbody, Gimmick gimmick) : base(rigidbody, gimmick)
    {
    }

    public override void OnCollision()
    {
        if (Gimmick.InvokeOnCollision)
        {
            Gimmick.Invoke(_rigidbody);
        }
    }
}

public class ItemCollision : GimmickCollision
{
    public Item Item => (Item)Other;
    public ItemCollision(MoonBunnyRigidbody rigidbody, Item item) : base(rigidbody, item)
    {
    }

    public override void OnCollision()
    {
        base.OnCollision();
        Item.Taken();
    }
}

public class ObstacleCollision : GimmickCollision
{
    public Obstacle Obstacle => (Obstacle)Other;
    public ObstacleCollision(MoonBunnyRigidbody rigidbody, Obstacle obstacle) : base(rigidbody, obstacle)
    {
    }
}


public class PlatformCollision : GimmickCollision
{
    public Platform Platform => (Platform)Other;
    public PlatformCollision(MoonBunnyRigidbody rigidbody, Platform platform) : base(rigidbody, platform)
    {
    }
}

public class BouncyPlatformCollision : PlatformCollision
{
    public static AudioClip JumpAudioClip;
    public BouncyPlatform BouncyPlatform => (BouncyPlatform)Other;
    public BouncyPlatformCollision(MoonBunnyRigidbody rigidbody, BouncyPlatform platform) : base(rigidbody, platform)
    {
        SoundManager.instance.PlayClip(JumpAudioClip);
    }
}
