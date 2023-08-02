#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.UI;

namespace MoonBunny.UIs
{
    public class ButtonWithSound : Button
    {
        public AudioClip ButtonSound;

#if UNITY_EDITOR
        protected override void Reset()
        {
            base.Reset();

            ButtonSound = AssetDatabase.LoadAssetAtPath<AudioClip>("Assets/Resources/Sounds/Button Click Sound.wav");
        }
#endif

        protected override void Start()
        {
            base.Start();

            onClick.AddListener(() => SoundManager.instance.PlayClip(ButtonSound));
        }
    }
}