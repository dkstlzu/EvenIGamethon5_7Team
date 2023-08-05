using System;
using System.Collections.Generic;
using MoonBunny.Effects;
using UnityEngine;
using UnityEngine.Pool;

namespace MoonBunny
{
    public class Item : Gimmick
    {
        public static event Action<Item> OnInvoke;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void StaticEventInit()
        {
            OnInvoke = null;
        }
        
        public int Score;
        [SerializeField] protected SpriteRenderer _renderer;
        [SerializeField] protected AudioClip _audioClip;
        
        public override bool Invoke(MoonBunnyRigidbody with, MoonBunnyCollider.Direction direction)
        {
            if (!base.Invoke(with, direction)) return false;
            
            if (_audioClip) SoundManager.instance.PlayClip(_audioClip);
            GameManager.instance.Stage.Score += Score;
            Destroy(gameObject);
            
            OnInvoke?.Invoke(this);
            return true;
        }
    }
}