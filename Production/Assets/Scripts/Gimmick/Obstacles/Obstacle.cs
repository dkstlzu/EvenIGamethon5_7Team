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

        public override bool Invoke(MoonBunnyRigidbody with)
        {
            if (!base.Invoke(with)) return false;

            Character target = with.GetComponent<Character>();
            if ((target.Friend.Name == FriendName.Lala || target.Friend.Name == FriendName.Soda) && this is PinWheel)
            {
                return false;
            }
            
            if (_audioClip) SoundManager.instance.PlayClip(_audioClip);
            with.GetComponent<Character>().Hit(this);
            return true;
        }
    }
}