using UnityEngine;
using UnityEngine.UI;

namespace MoonBunny.UIs
{
    public class SpriteSwappableToggle : Toggle
    {
        public Image SwapTargetImage;
        public Sprite ToggleOnSprite;
        public Sprite ToggleOffSprite;
        
        protected override void Awake()
        {
            base.Awake();

            onValueChanged.AddListener(OnToggle);
        }

        private void OnToggle(bool on)
        {
            if (!SwapTargetImage) return;
            
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