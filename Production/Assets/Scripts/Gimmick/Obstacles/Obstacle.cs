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
            set
            {
                _type = value;
                _renderer.sprite = PreloadedResources.instance.ObstacleSpriteList[(int)_type];

                ObstacleSpec spec = PreloadedResources.instance.ObstacleSpecList[(int)_type];
            }
        }

        [SerializeField] protected AudioSource _audioSource;
        [SerializeField] protected SpriteRenderer _renderer;

        public override void Invoke(MoonBunnyRigidbody with)
        {
            base.Invoke(with);

            Character character;
            if (with.TryGetComponent<Character>(out character))
            {
                character.CurrentHp--;
            }
        }
    }
}