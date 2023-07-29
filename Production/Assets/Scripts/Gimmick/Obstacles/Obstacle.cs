using System;
using MoonBunny.Dev;
using MoonBunny.Effects;
using UnityEngine;

namespace MoonBunny
{
    public class Obstacle : Gimmick
    {
        [SerializeField] protected SpriteRenderer _renderer;
        [SerializeField] protected AudioClip _audioClip;

        public override bool Invoke(MoonBunnyRigidbody with, MoonBunnyCollider.Direction direction)
        {
            if (!base.Invoke(with, direction)) return false;

            Character target = with.GetComponent<Character>();
            if (target.Friend.Name == FriendName.Lala && this is PinWheel)
            {
                return false;
            }
            
            if (_audioClip) SoundManager.instance.PlayClip(_audioClip);
            with.GetComponent<Character>().Hit(this);
            return true;
        }

        public override Collision[] Collide(MoonBunnyRigidbody rigidbody, MoonBunnyCollider.Direction direction)
        {
            if (rigidbody.GridObject is Character character && (direction & MoonBunnyCollider.Direction.Down) > 0)
            {
                if (character.Friend.Name is FriendName.BlackSugar)
                {
                    return new Collision[]
                    {
                        new BounceCollision(rigidbody, this),
                        new DestroyCollision(rigidbody, this),
                    };
                }
            }
            
            return base.Collide(rigidbody, direction);
        }
    }
}