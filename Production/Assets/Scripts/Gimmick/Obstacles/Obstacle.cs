using System;
using UnityEngine;

namespace MoonBunny
{
    public enum ObstacleType
    {
        None = -1,
        PinWheel = 0,
        SpiderWeb,
        Bee,
        CarnivorousFlower,
        Crow,
        Thunder,
        ShootingStar,
    }
    
    public class Obstacle : Gimmick
    {
        [HideInInspector][SerializeField] private ObstacleType _type;

        public ObstacleType Type
        {
            get => _type;
        }

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