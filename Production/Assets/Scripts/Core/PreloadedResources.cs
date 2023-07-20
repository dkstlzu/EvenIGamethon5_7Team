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
        public GameObject StarCandyEffectPrefab;
        public GameObject ThunderEffectPrefab;
        public GameObject WarningEffectPrefab;

        private void Awake()
        {
            BouncyPlatform.S_JumpAudioClip = BouncyPlatformSound;
            StarCandyEffect.S_StarCandyExplosionEffect = StarCandyEffectPrefab;
            ThunderEffect.S_ThunderEffectPrefab = ThunderEffectPrefab;
            WarningEffect.S_WarningEffectPrefab = WarningEffectPrefab;
        }
    }
}