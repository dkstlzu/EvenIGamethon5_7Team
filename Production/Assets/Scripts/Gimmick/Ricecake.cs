using UnityEngine;

namespace MoonBunny
{
    public class Ricecake : Gimmick
    {
        public int Score;

        private const int NormalCakeScore = 10;
        private const int RainbowCakeScore = 50;

        [SerializeField] private SpriteRenderer _headRenerer;
        [SerializeField] private SpriteRenderer _topRenerer;
        [SerializeField] private SpriteRenderer _middleRenerer;
        [SerializeField] private SpriteRenderer _bottomRenerer;

        public void MakeRainbow()
        {
            _headRenerer.sprite = PreloadedResources.instance.RicecakeSpriteList[4];
            _topRenerer.sprite = PreloadedResources.instance.RicecakeSpriteList[1];
            _middleRenerer.sprite = PreloadedResources.instance.RicecakeSpriteList[2];
            _bottomRenerer.sprite = PreloadedResources.instance.RicecakeSpriteList[3];

            Score = RainbowCakeScore;
        }

        public override void Invoke()
        {
            print("Ricecake Invoke");
            GameManager.instance.Score += Score;
            Destroy(gameObject);
        }
    }
}