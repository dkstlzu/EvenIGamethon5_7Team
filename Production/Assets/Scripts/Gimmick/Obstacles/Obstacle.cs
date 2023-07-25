using System;
using UnityEngine;

namespace MoonBunny
{
    public class Obstacle : Gimmick
    {
        [SerializeField] protected SpriteRenderer _renderer;
        [SerializeField] protected AudioClip _audioClip;

        public override void Invoke(MoonBunnyRigidbody with)
        {
            base.Invoke(with);
            
            if (_audioClip) SoundManager.instance.PlayClip(_audioClip);
            with.GetComponent<Character>().Hit(this);
        }
    }
}