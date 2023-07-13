using System;
using UnityEngine;

namespace MoonBunny
{
    public enum ObstacleType
    {
        None = -1,
        PinWheel = 0,
        SpiderWeb,
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

        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private SpriteRenderer _renderer;

    }
}