using System;
using MoonBunny.Effects;
using UnityEngine;
using UnityEngine.Pool;

namespace MoonBunny
{
    public class Item : Gimmick
    {
        public static event Action OnInvoke;

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
            _renderer.enabled = false;
            GetComponent<Collider2D>().enabled = false;
            Destroy(gameObject, 2);
            return true;
        }
    }
}