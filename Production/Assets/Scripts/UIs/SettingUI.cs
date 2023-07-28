using System;
using UnityEngine;

namespace MoonBunny.UIs
{
    public class SettingUI : UI
    {
        public SpriteSwappableToggle SoundToggle;
        public SpriteSwappableToggle TutorialToggle;

        private void Awake()
        {
            SoundToggle.onValueChanged.AddListener(OnSoundToggle);
            TutorialToggle.onValueChanged.AddListener(OnTutorialToggle);
        }

        private void Start()
        {
            GameManager.instance.SaveLoadSystem.OnSaveDataLoaded += () =>
            {
                TutorialToggle.isOn = GameManager.instance.SaveLoadSystem.SaveData.ShowTutorial;
            };
        }

        void OnSoundToggle(bool on)
        {
            AudioListener.volume = on ? 1 : 0;
        }

        void OnTutorialToggle(bool on)
        {
            GameManager.instance.ShowTutorial = on;
        }

        public void OnCreditButtonClicked()
        {
            
        }

        public void OnResetDataButtonClicked()
        {
            
        }
    }
}