using System;
using System.Collections.Generic;
using dkstlzu.Utility;
using GooglePlayGames.BasicApi;
using MoonBunny.Dev;
using MoonBunny.UIs;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

namespace MoonBunny
{
    [DefaultExecutionOrder(-10)]
    public class GameManager : Singleton<GameManager>
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        static void Init()
        {
            TimeUpdatable.GlobalSpeed = 1;
            TimeUpdatable.Enabled = true;
            if (instance)
            {
                instance.OnStageSceneLoaded += () => TimeUpdatable.GlobalSpeed = 1;
                instance.OnStageSceneUnloaded += () => TimeUpdatable.GlobalSpeed = 1;
            }
        }

        public static ProgressSaveData ProgressSaveData
        {
            get
            {
                if (!instance.SaveLoadSystem.DataIsLoaded)
                {
                    Debug.LogWarning("ProgressSaveData in GameManager is not loaded yet");
                }
                
                return instance.SaveLoadSystem.ProgressSaveData;
            }
            set
            {
                instance.SaveLoadSystem.DataIsLoaded = true;
                instance.SaveLoadSystem.ProgressSaveData = value;
            }
        }

        public SceneLoadCallbackSetter SCB;
        public SaveLoadSystem SaveLoadSystem;

        public Stage Stage;

        public StartSceneUI StartSceneUI;

        public FriendName UsingFriendName
        {
            get => Enum.Parse<FriendName>(ProgressSaveData.UsingFriendName);
            set => ProgressSaveData.UsingFriendName = value.ToString();
        }

        public int DiamondNumber
        {
            get => ProgressSaveData.DiamondNumber;
            set
            {
                ProgressSaveData.DiamondNumber = value;
                if (StartSceneUI)
                {
                    StartSceneUI.DiamondText1.text = value.ToString();
                    StartSceneUI.DiamondText2.text = value.ToString();
                }
            }
        }

        public int GoldNumber
        {
            get => ProgressSaveData.GoldNumber;
            set
            {
                ProgressSaveData.GoldNumber = value;
                if (StartSceneUI)
                {
                    StartSceneUI.GoldText1.text = value.ToString();
                    StartSceneUI.GoldText2.text = value.ToString();
                }
            }
        }

        public bool ShowTutorial
        {
            get => ProgressSaveData.ShowTutorial;
            set => ProgressSaveData.ShowTutorial = value;
        }
        
        public float VolumeSetting
        {
            get => ProgressSaveData.VolumeSetting;
            set
            {
                ProgressSaveData.VolumeSetting = value;
                AudioListener.volume = value;
            }
        }

        public bool RemoveAd;

        public event Action OnStageSceneLoaded;
        public event Action OnStageSceneUnloaded;
        
#if UNITY_EDITOR
        public bool useSaveSystem;
#endif
        
        private void Awake()
        {
            Application.targetFrameRate = 60;
            
#if UNITY_EDITOR
            if (useSaveSystem)
            {
                SaveLoadSystem.Init("Saves", "Save", "txt");
                SaveLoadSystem.LoadProgress();
            }
            else
            {
                SaveLoadSystem.DataIsLoaded = true;
                SaveLoadSystem.ProgressSaveData = ProgressSaveData.GetDefaultSaveData();
            }
#endif

            SCB = new SceneLoadCallbackSetter(SceneName.Names);

            for (int i = 0; i < SceneName.StageNames.Length; i++)
            {
                SCB.SceneLoadCallBackDict[SceneName.StageNames[i]] += () =>
                {
                    OnStageSceneLoaded?.Invoke();
                };
                SCB.SceneUnloadCallBackDict[SceneName.StageNames[i]] += () =>
                {
                    OnStageSceneUnloaded?.Invoke();
                };
            }

            SaveLoadSystem.OnSaveDataLoaded += () =>
            {
                foreach (string sceneName in SceneName.Names)
                {
                    SCB.SceneLoadCallBackDict[sceneName] += () =>
                    {
                        AudioListener.volume = VolumeSetting;
                    };
                }
            };
            
            IAPManager.instance.OnPurchaseComplete += (productName) =>
            {
                if (productName == "패키지")
                {
                    ProgressSaveData.UnlimitedPackagePurchasedNumber++;
                }
                else if (productName == "한정 패키지")
                {
                    ProgressSaveData.LimitedPackagePurchased = true;
                }
            };
        }

        public void QuitGame()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
                Application.Quit();
#endif
        }

        private void OnApplicationQuit()
        {
#if UNITY_EDITOR
            SaveProgress();
#endif
        }

        public void SaveProgress()
        {
#if UNITY_EDITOR
            if (!useSaveSystem) return;
#endif
            if (!SaveLoadSystem.DataIsLoaded)
            {
                Debug.LogError("Cannot save progress. SaveLoad system never loaded data");
                return;
            }
            
            SaveLoadSystem.SaveProgress();
        }

        public void RestartApplication()
        {
            LoadingScene.LoadScene(SceneName.Loading);
        }
    }
}