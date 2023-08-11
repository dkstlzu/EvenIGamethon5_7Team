using System;
using UnityEngine;

namespace MoonBunny.UIs
{
    public class SettingUI : UI
    {
        public SpriteSwappableToggle SoundToggle;
        public SpriteSwappableToggle TutorialToggle;

        public ConfirmUI ConfirmUI;
        public string ConfirmDescriptionText = 
            @"정말 초기화 하겠습니까?

이는 영구적으로 초기화되며
되돌릴 수 없습니다.";

        protected override void Awake()
        {
            base.Awake();
            
            SoundToggle.onValueChanged.AddListener(OnSoundToggle);
            TutorialToggle.onValueChanged.AddListener(OnTutorialToggle);
        }

        protected override void Rebuild()
        {
            TutorialToggle.isOn = GameManager.ProgressSaveData.ShowTutorial;
            SoundToggle.isOn = GameManager.ProgressSaveData.VolumeSetting > 0;
        }

        void OnSoundToggle(bool on)
        {
            GameManager.instance.VolumeSetting = on ? 1 : 0;
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
            ConfirmUI.Description.text = ConfirmDescriptionText;
            ConfirmUI.AddConfirmListener(DataReset);
            ConfirmUI.Open();
        }

        void DataReset()
        {
            bool progressReset = false;
            bool questReset = false;
            
            GameManager.instance.SaveLoadSystem.ProgressSaveData = ProgressSaveData.GetDefaultSaveData();
            GameManager.instance.SaveLoadSystem.OnSaveSuccess += () =>
            {
                progressReset = true;

                if (progressReset && questReset)
                {
#if UNITY_EDITOR
                    UnityEditor.EditorApplication.isPlaying = false;
#else
                    Application.Quit();
#endif
                }
            };
            GameManager.instance.SaveProgress();
            
            QuestManager.instance.SaveLoadSystem.QuestSaveData = QuestSaveData.GetDefaultSaveData();
            QuestManager.instance.SaveLoadSystem.OnSaveSuccess += () =>
            {
                questReset = true;

                if (progressReset && questReset)
                {
#if UNITY_EDITOR
                    UnityEditor.EditorApplication.isPlaying = false;
#else
                    Application.Quit();
#endif
                }
            };
            QuestManager.instance.SaveLoadSystem.SaveQuest();
        }
    }
}