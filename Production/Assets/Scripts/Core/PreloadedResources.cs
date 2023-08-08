﻿using System.Collections.Generic;
using dkstlzu.Utility;
using MoonBunny.Effects;
using UnityEngine;

namespace MoonBunny
{
    [DefaultExecutionOrder(-10)]
    public class PreloadedResources : Singleton<PreloadedResources>
    {
        public List<Sprite> BouncyPlatformSpriteList;
        public List<Sprite> LevelEndPlatformSpriteList;
        public List<RuntimeAnimatorController> BouncyPlatformAnimatorControllerList;
        public List<RuntimeAnimatorController> CrackPlatformAnimatorControllerList;
        public List<Sprite> CrackPlatformSpriteList;
        public List<RuntimeAnimatorController> CharacterAnimatorControllerList;
        public List<FriendSpec> FriendSpecList;
        public AudioClip BouncyPlatformSound;
        public AudioClip OpenStageAudioClip;
        public AudioClip FriendCollectedAudioClip;
        public List<Sprite> MemorySpriteList;

        public GameObject CannibalismEffectPrefab;
        public GameObject StarCandyEffectPrefab;
        public GameObject ThunderEffectPrefab;
        public GameObject ThunderImpactEffectPrefab;
        public GameObject WarningEffectPrefab;
        public GameObject ShootingStarEffectPrefab;
        public GameObject CoinPrefab;

        private void Awake()
        {
            BouncyPlatform.S_JumpAudioClip = BouncyPlatformSound;
            FriendCollectable.S_FriendCollectedAudioClip = FriendCollectedAudioClip;
            
            CannibalismFlower.S_EffectPrefab = CannibalismEffectPrefab;
            StarCandyEffect.S_StarCandyExplosionEffect = StarCandyEffectPrefab;
            ThunderEffect.S_ThunderEffectPrefab = ThunderEffectPrefab;
            ThunderEffect.S_ThunderImpactEffectPrefab = ThunderImpactEffectPrefab;
            WarningEffect.S_WarningEffectPrefab = WarningEffectPrefab;
            ShootingStarEffect.S_ShootingStarEffectPrefab = ShootingStarEffectPrefab;
            StarCandyBoostEffect.S_CoinPrefab = CoinPrefab;
        }
    }
}