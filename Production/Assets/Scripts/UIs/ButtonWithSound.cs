#if UNITY_EDITOR
using UnityEditor;
#endif
using dkstlzu.Utility;
using UnityEngine;
using UnityEngine.UI;

namespace MoonBunny.UIs
{
    public class ButtonWithSound : Button
    {
        public AudioClip ButtonSound;
        public float CoolTime;

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

            if (CoolTime > 0)
            {
                onClick.AddListener(() =>
                {
                    interactable = false;
                    CoroutineHelper.Delay(() =>
                    {
                        interactable = true;
                    }, CoolTime);
                });
            }
            onClick.AddListener(() => SoundManager.instance.PlayClip(ButtonSound));
        }
    }
}