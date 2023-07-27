using System;
using System.Collections.Generic;
using MoonBunny;
using UnityEngine;
using Object = UnityEngine.Object;

public class Collision : IEqualityComparer<Collision>
{
    public class CollisionArgs
    {
        public FieldObject This { get; private set; }
        public FieldObject Other { get; private set; }

        public CollisionArgs(FieldObject @this, FieldObject other)
        {
            This = @this;
            Other = other;
        }
    }

    private FieldObject _other;
    public FieldObject Other => _other;
    private FieldObject _this;
    public FieldObject This => _this;

    public Collision(CollisionArgs args)
    {
        _this = args.This;
        _other = args.Other;
    }

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

public class GimmickCollision : Collision
{
    public class GimmickCollisionArgs : CollisionArgs
    {
        public MoonBunnyRigidbody Rigidbody { get; protected set;}
        public GimmickCollisionArgs(MoonBunnyRigidbody rigidbody, FieldObject @this, Gimmick other) : base(@this, other)
        {
            Rigidbody = rigidbody;
        }
    }
    
    public Gimmick Gimmick => (Gimmick)Other;
    private MoonBunnyRigidbody _rigidbody;
    
    public GimmickCollision(GimmickCollisionArgs args) : base(args)
    {
        _rigidbody = args.Rigidbody;
    }

    public override void OnCollision()
    {
        if (Gimmick.InvokeOnCollision)
        {
            Gimmick.Invoke(_rigidbody);
        }
    }
}

public class PlatformCollision : Collision
{
    public class PlatformCollisionArgs : CollisionArgs
    {
        public MoonBunnyRigidbody Rigidbody { get; protected set; }

        public PlatformCollisionArgs(Character @this, FieldObject other) : base(@this, other)
        {
            Rigidbody = @this.GetComponent<MoonBunnyRigidbody>();
        }
    }

    public Platform Platform => (Platform)Other;
    private MoonBunnyRigidbody _rigidbody;

    public PlatformCollision(PlatformCollisionArgs args) : base(args)
    {
        _rigidbody = args.Rigidbody;
    }
}


public class DirectionCollision : Collision
{
    public class DirectionCollisionArgs : CollisionArgs
    {
        public MoonBunnyCollider.Direciton Direciton { get; private set; }
            
        public DirectionCollisionArgs(MoonBunnyCollider.Direciton direciton, FieldObject @this, FieldObject other) : base(@this, other)
        {
            Direciton = direciton;
        }
    }
        
    private MoonBunnyCollider.Direciton _direction;
    public MoonBunnyCollider.Direciton Direciton => _direction;

    public DirectionCollision(DirectionCollisionArgs args) : base(args.This, args.Other)
    {
        _direction = args.Direciton;
    }

    public override void OnCollision()
    {
        base.OnCollision();

        if (This is Character character && Other is Obstacle obstacle)
        {
            if ((_direction & MoonBunnyCollider.Direciton.Down) > 0)
            {
                MonoBehaviour.Destroy(obstacle.gameObject);
            }
        }
    }
}
