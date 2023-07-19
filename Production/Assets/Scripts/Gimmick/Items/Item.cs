using System;
using UnityEngine;
using UnityEngine.Pool;

namespace MoonBunny
{
    public enum ItemType
    {
        None = -1,
        LuckyClover = 0,
        Butterfly,
        FairyWing,
        CottonCandy,
        DreamCatcher,
        Rocket,
        Magnet,
        Heart,
        StarCandy,
    }
    
    public class Item : Gimmick
    {
        [HideInInspector][SerializeField] private ItemType _type;
        public ItemType Type
        {
            get => _type;
        }
        
        public int Score;
        [SerializeField] protected SpriteRenderer _renderer;
        [SerializeField] protected AudioClip _audioClip;
        
        public static event Action<Item> OnItemTaken;

        public virtual void Taken()
        {
            OnItemTaken?.Invoke(this);
            if (_audioClip) SoundManager.instance.PlayClip(_audioClip);
            _renderer.enabled = false;
            GetComponent<Collider2D>().enabled = false;
            Destroy(gameObject, 2);
        }
    }
}