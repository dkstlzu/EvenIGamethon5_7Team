using UnityEngine;

namespace MoonBunny
{
    public class Ricecake : Gimmick
    {
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

        public override bool Invoke(MoonBunnyRigidbody with)
        {
            if (!base.Invoke(with)) return false;

            GameManager.instance.Stage.Score += Score;
            SoundManager.instance.PlayClip(_audioClip);
            Destroy(gameObject);
            return true;
        }
    }
}