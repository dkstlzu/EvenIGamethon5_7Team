#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.UI;

namespace MoonBunny.UIs
{
    public class SpriteSwappableToggle : Toggle
    {
        public AudioClip ButtonSound;
        public Image SwapTargetImage;
        public Sprite ToggleOnSprite;
        public Sprite ToggleOffSprite;

#if UNITY_EDITOR
        protected override void Reset()
        {
            base.Reset();

            ButtonSound = AssetDatabase.LoadAssetAtPath<AudioClip>("Assets/Resources/Sounds/Button Click Sound.wav");
        }
#endif
        
        protected override void Awake()
        {
            base.Awake();

            onValueChanged.AddListener(OnToggle);
        }

        private void OnToggle(bool on)
        {
            if (!SwapTargetImage) return;
            
            SoundManager.instance?.PlayClip(ButtonSound);
            
            if (on)
            {
                SwapTargetImage.sprite = ToggleOnSprite;
            }
            else
            {
                SwapTargetImage.sprite = ToggleOffSprite;
            }
        }
    }
}