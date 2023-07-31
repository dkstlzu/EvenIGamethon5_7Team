using System;
using System.Collections.Generic;
using dkstlzu.Utility;
using MoonBunny.Dev;
using MoonBunny.UIs;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

namespace MoonBunny
{
    [DefaultExecutionOrder(-1)]
    public class GameManager : Singleton<GameManager>
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        static void Init()
        {
            TimeUpdatable.GlobalSpeed = 1;
            TimeUpdatable.Enabled = true;
            instance.OnStageSceneLoaded += () => TimeUpdatable.GlobalSpeed = 1;
            instance.OnStageSceneUnloaded += () => TimeUpdatable.GlobalSpeed = 1;
        }

        public static SaveData SaveData
        {
            get
            {
                if (!instance.SaveLoadSystem.DataIsLoaded) return null;
                else return instance.SaveLoadSystem.SaveData;
            }
        }

        public SceneLoadCallbackSetter SCB;
        public SaveLoadSystem SaveLoadSystem;

        public Stage Stage;

        public StartSceneUI StartSceneUI;

        public FriendName UsingFriendName
        {
            get => Enum.Parse<FriendName>(SaveData.UsingFriendName);
            set => SaveData.UsingFriendName = value.ToString();
        }

        private int _diamondNumber;
        public int DiamondNumber
        {
            get => _diamondNumber;
            set
            {
                _diamondNumber = value;
                if (StartSceneUI)
                {
                    StartSceneUI.DiamondText1.text = value.ToString();
                    StartSceneUI.DiamondText2.text = value.ToString();
                }
            }
        }

        public int GoldNumber
        {
            get => SaveData.GoldNumber;
            set
            {
                SaveData.GoldNumber = value;
                if (StartSceneUI)
                {
                    StartSceneUI.GoldText1.text = value.ToString();
                    StartSceneUI.GoldText2.text = value.ToString();
                }
            }
        }

        public bool ShowTutorial
        {
            get => SaveData.ShowTutorial;
            set => SaveData.ShowTutorial = value;
        }
        
        public float VolumeSetting
        {
            get => SaveData.VolumeSetting;
            set
            {
                SaveData.VolumeSetting = value;
                AudioListener.volume = value;
            }
        }

        public event Action OnStageSceneLoaded;
        public event Action OnStageSceneUnloaded;
        
#if UNITY_EDITOR
        public bool useSaveSystem;
#endif
        
        private void Awake()
        {
#if UNITY_EDITOR
            if (useSaveSystem)
            {
#endif
                SaveLoadSystem.LoadDatabase();
#if UNITY_EDITOR
            }
#endif

            SCB = new SceneLoadCallbackSetter(SceneName.Names);



            for (int i = 2; i < SceneName.Names.Length; i++)
            {
                SCB.SceneLoadCallBackDict[SceneName.Names[i]] += () =>
                {
                    OnStageSceneLoaded?.Invoke();
                };
                SCB.SceneUnloadCallBackDict[SceneName.Names[i]] += () =>
                {
                    OnStageSceneUnloaded?.Invoke();
                };
            }
        }

        private void OnApplicationQuit()
        {
            SaveProgress();
        }

        public void SaveProgress()
        {
#if UNITY_EDITOR
            if (!useSaveSystem) return;
#endif
            if (!SaveLoadSystem.DataIsLoaded)
            {
                Debug.LogError("Cannot save progress. SaveLoadsystem never loaded data");
                return;
            }
            
            SaveLoadSystem.SaveDatabase();
        }

        public void RestartApplication()
        {
            SceneManager.LoadScene(SceneName.Loading);
        }
    }
}