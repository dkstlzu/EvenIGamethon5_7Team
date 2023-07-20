using System;
using UnityEngine;

namespace MoonBunny.Effects
{
    public class Warning : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _renderer; 
        
        private Vector2 _size;
        public Vector2 Size
        {
            get => _size;
            set
            {
                _size = value;
                _renderer.size = value;
            }
        }

        public float Duration;

        private float _timer;
        private void Update()
        {
            _timer += Time.deltaTime;

            if (_timer > Duration)
            {
                Destroy(gameObject);
            }
        }
    }
}