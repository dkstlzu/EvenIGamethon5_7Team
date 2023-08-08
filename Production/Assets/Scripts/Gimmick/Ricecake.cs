using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace MoonBunny
{
    public class Ricecake : Gimmick
    {
        public static float S_RainbowRatio;
        
        public int Score;

        private const int NormalCakeScore = 10;
        private const int RainbowCakeScore = 50;

        [SerializeField] private SpriteRenderer _renderer;
        [SerializeField] private AudioClip _audioClip;

        [SerializeField] private Sprite _normalRiceCakeSprite;
        [SerializeField] private Sprite _rainBowRiceCakeSprite;
        
        public void MakeRainbow()
        {
            _renderer.sprite = _rainBowRiceCakeSprite;

            Score = RainbowCakeScore;
        }

        private void Start()
        {
            if (Random.value <= S_RainbowRatio)
            {
                MakeRainbow();
            }
        }

        public override bool Invoke(MoonBunnyRigidbody with, MoonBunnyCollider.Direction direction)
        {
            if (!base.Invoke(with, direction)) return false;

            GameManager.instance.Stage.Score += Score;
            SoundManager.instance.PlayClip(_audioClip);
            Destroy(gameObject);
            return true;
        }
    }
}