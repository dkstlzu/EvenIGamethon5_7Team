using System;
using UnityEngine;

namespace MoonBunny
{
    public enum ObstacleType
    {
        None = -1,
        Block = 0,
    }
    
    public class Obstacle : MonoBehaviour
    {
        [SerializeField] private ObstacleType _type;

        public ObstacleType Type
        {
            get => _type;
            set
            {
                _type = value;
                _renderer.sprite = PreloadedResources.instance.ObstacleSpriteList[(int)_type];

                ObstacleSpec spec = PreloadedResources.instance.ObstacleSpecList[(int)_type];
                Damage = spec.Damage;
            }
        }

        public int Damage;

        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private SpriteRenderer _renderer;

        public static event Action<Obstacle> OnObstacleCollision;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.tag == "Player")
            {
                OnObstacleCollision?.Invoke(this);
            }
        }
    }
}