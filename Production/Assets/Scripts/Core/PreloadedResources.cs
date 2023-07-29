using System.Collections.Generic;
using dkstlzu.Utility;
using MoonBunny.Effects;
using UnityEngine;

namespace MoonBunny
{
    [DefaultExecutionOrder(-10)]
    public class PreloadedResources : Singleton<PreloadedResources>
    {
        public List<Sprite> BouncyPlatformSpriteList;
        public List<RuntimeAnimatorController> BouncyPlatformAnimatorControllerList;
        public AudioClip BouncyPlatformSound;
        public AudioClip OpenStageAudioClip;
        public AudioClip FriendCollectedAudioClip;

        public Sprite SpiderWebDebuffSprite;
        public GameObject CannibalismEffectPrefab;
        public GameObject StarCandyEffectPrefab;
        public GameObject ThunderEffectPrefab;
        public GameObject WarningEffectPrefab;
        public GameObject ShootingStarEffectPrefab;
        public GameObject CoinPrefab;

        private void Awake()
        {
            BouncyPlatform.S_JumpAudioClip = BouncyPlatformSound;
            FriendCollectable.S_FriendCollectedAudioClip = FriendCollectedAudioClip;
            
            SlowEffect.SpiderWebDebuffSprite = SpiderWebDebuffSprite;
            CannibalismFlower.S_EffectPrefab = CannibalismEffectPrefab;
            StarCandyEffect.S_StarCandyExplosionEffect = StarCandyEffectPrefab;
            ThunderEffect.S_ThunderEffectPrefab = ThunderEffectPrefab;
            WarningEffect.S_WarningEffectPrefab = WarningEffectPrefab;
            ShootingStarEffect.S_ShootingStarEffectPrefab = ShootingStarEffectPrefab;
            StarCandyBoostEffect.S_CoinPrefab = CoinPrefab;
        }
    }
}