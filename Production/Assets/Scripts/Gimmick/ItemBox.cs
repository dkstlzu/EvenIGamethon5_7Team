using MoonBunny.Effects;
using UnityEngine;

namespace MoonBunny
{
    public class ItemBox : Gimmick
    {
        public Sprite HeartSprite;
        [Range(0, 1)] public float HeartPotential;
        public Sprite StarCandySprite;
        [Range(0, 1)] public float StarCandyPotential;
        public Sprite CoinSprite;
        [Range(0, 1)] public float CoinPotential;

        public Transform SpawnPoint;
        [SerializeField] private AudioClip _audioClip;

        private float _totalPotential => HeartPotential + StarCandyPotential + CoinPotential;

        public override bool Invoke(MoonBunnyRigidbody with, MoonBunnyCollider.Direction direction)
        {
            if (!base.Invoke(with, direction)) return false;

            float randomValue = Random.Range(0, _totalPotential);
            Sprite targetSprite = null;

            if (randomValue < HeartPotential)
            {
                targetSprite = HeartSprite;
                new HeartEffect(Character.instance).Effect();
            } else if (randomValue >= HeartPotential && randomValue < (HeartPotential + StarCandyPotential))
            {
                targetSprite = StarCandySprite;
                new StarCandyEffect(LayerMask.GetMask("Obstacle"), new Bounds(transform.position, new Vector3(40, GridTransform.GridSetting.GridHeight * 3))).Effect();
            } else
            {
                targetSprite = CoinSprite;
                GameManager.instance.Stage.GoldNumber++;
            }

            if (targetSprite != null)
            {
                new SpriteEffect(targetSprite, SpawnPoint.position, 1.5f, 2, 2).Effect();
            }
            else
            {
                print("Something wrong on itembox");
            }
            
            SoundManager.instance.PlayClip(_audioClip);
            Destroy(gameObject);
            return true;
        }
    }
}