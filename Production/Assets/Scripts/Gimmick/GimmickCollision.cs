using System;
using System.Collections.Generic;
using MoonBunny;
using MoonBunny.Effects;
using UnityEngine;
using Object = UnityEngine.Object;

public class Collision : IEqualityComparer<Collision>
{

    private FieldObject _other;
    public FieldObject Other => _other;
    private FieldObject _this;
    public FieldObject This => _this;

    protected Collision(FieldObject @this, FieldObject other)
    {
        _this = @this;
        _other = other;
    }

    public virtual void OnCollision()
    {

    }

    public bool Equals(Collision x, Collision y)
    {
        if (ReferenceEquals(x, y)) return true;
        if (ReferenceEquals(x, null)) return false;
        if (ReferenceEquals(y, null)) return false;
        if (x.GetType() != y.GetType()) return false;
        return Equals(x._other, y._other) && Equals(x._this, y._this);
    }

    public int GetHashCode(Collision obj)
    {
        return HashCode.Combine(obj._other, obj._this);
    }
}

public class MoonBunnyCollision : Collision
{
    protected MoonBunnyCollision(MoonBunnyRigidbody rigidbody, FieldObject other) : base(rigidbody.GridObject, other)
    {
    }
}

public class GimmickCollision : MoonBunnyCollision
{
    public Gimmick Gimmick => (Gimmick)Other;
    private MoonBunnyRigidbody _rigidbody;
    private MoonBunnyCollider.Direction _direction;
    
    public GimmickCollision(MoonBunnyRigidbody rigidbody, MoonBunnyCollider.Direction direction, Gimmick gimmick) : base(rigidbody, gimmick)
    {
        _rigidbody = rigidbody;
        _direction = direction;
    }

    public override void OnCollision()
    {
        if (Gimmick.InvokeOnCollision)
        {
            Gimmick.Invoke(_rigidbody, _direction);
        }
    }
}

public class BlockCollision : MoonBunnyCollision
{
    public BlockCollision(MoonBunnyRigidbody rigidbody, FieldObject other) : base(rigidbody, other)
    {
    }
}

public class BounceCollision : MoonBunnyCollision
{
    public BounceCollision(MoonBunnyRigidbody rigidbody, FieldObject other) : base(rigidbody, other)
    {
    }
}

public class FlipCollision : MoonBunnyCollision
{
    public bool isVertical;
    public FlipCollision(bool isVertical, MoonBunnyRigidbody rigidbody, FieldObject other) : base(rigidbody, other)
    {
        this.isVertical = isVertical;
    }
}

public class BouncyPlatformCollision : MoonBunnyCollision
{
    public BouncyPlatform BouncyPlatform => (BouncyPlatform)Other;
    public BouncyPlatformCollision(MoonBunnyRigidbody rigidbody, BouncyPlatform bouncyPlatform) : base(rigidbody, bouncyPlatform)
    {
    }
}

public class DestroyCollision : MoonBunnyCollision
{
    public DestroyCollision(MoonBunnyRigidbody rigidbody, FieldObject other) : base(rigidbody, other)
    {
    }

    public override void OnCollision()
    {
        MonoBehaviour.Instantiate(StarCandyEffect.S_StarCandyExplosionEffect, Other.transform.position, Quaternion.identity);
        MonoBehaviour.Destroy(Other.gameObject);
    }
}

//
// public class DirectionCollision : Collision
// {
//     public class DirectionCollisionArgs : CollisionArgs
//     {
//         public MoonBunnyCollider.Direciton Direciton { get; private set; }
//             
//         public DirectionCollisionArgs(MoonBunnyCollider.Direciton direciton, FieldObject @this, FieldObject other) : base(@this, other)
//         {
//             Direciton = direciton;
//         }
//     }
//         
//     private MoonBunnyCollider.Direciton _direction;
//     public MoonBunnyCollider.Direciton Direciton => _direction;
//
//     public DirectionCollision(DirectionCollisionArgs args) : base(args.This, args.Other)
//     {
//         _direction = args.Direciton;
//     }
//
//     public override void OnCollision()
//     {
//         base.OnCollision();
//
//         if (This is Character character && Other is Obstacle obstacle)
//         {
//             if ((_direction & MoonBunnyCollider.Direciton.Down) > 0)
//             {
//                 MonoBehaviour.Destroy(obstacle.gameObject);
//             }
//         }
//     }
// }
