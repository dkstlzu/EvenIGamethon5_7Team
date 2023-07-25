using System;
using MoonBunny.Effects;
using UnityEngine;
using UnityEngine.Pool;

namespace MoonBunny
{
    public class Item : Gimmick
    {
        public int Score;
        [SerializeField] protected SpriteRenderer _renderer;
        [SerializeField] protected AudioClip _audioClip;
        
        public override void Invoke(MoonBunnyRigidbody with)
        {
            base.Invoke(with);
            
            if (_audioClip) SoundManager.instance.PlayClip(_audioClip);
            GameManager.instance.Stage.Score += Score;
            _renderer.enabled = false;
            GetComponent<Collider2D>().enabled = false;
            Destroy(gameObject, 2);
        }
    }
}