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
    }
    
    public class Item : MonoBehaviour
    {
        [SerializeField] private ItemType _type;
        public ItemType Type
        {
            get => _type;
            set
            {
                _type = value;
                _renderer.sprite = PreloadedResources.instance.ItemSpriteList[(int)_type];

                ItemSpec spec = PreloadedResources.instance.ItemSpecList[(int)_type];
                
                Score = spec.Score;
                _audioSource.clip = spec.AudioClip;
            }
        }
        
        public int Score;
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private SpriteRenderer _renderer;

        public static event Action<Item> OnItemCollision;
        public static event Action<Item> OnItemTaken;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.tag == "Player")
            {
                OnItemCollision?.Invoke(this);
            }
        }

        public virtual void Taken()
        {
            OnItemTaken?.Invoke(this);
            _audioSource.Play();
            _renderer.enabled = false;
        }
    }
}