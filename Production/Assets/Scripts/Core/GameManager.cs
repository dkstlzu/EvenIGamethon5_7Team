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

        public static ProgressSaveData SaveData
        {
            get
            {
                if (instance.SaveLoadSystem.Validation == SaveLoadSystem.DataValidation.Invalidate)
                {
                    Debug.LogWarning("ProgressSaveData in GameManager is not loaded yet");
                }
                
                return instance.SaveLoadSystem.ProgressSaveData;
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

        public int DiamondNumber
        {
            get => SaveData.DiamondNumber;
            set
            {
                SaveData.DiamondNumber = value;
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

        public bool RemoveAd
        {
            get => SaveData.RemoveAd;
            set => SaveData.RemoveAd = value;
        }

        public event Action OnStageSceneLoaded;
        public event Action OnStageSceneUnloaded;
        
        public bool useSaveSystem;
        
        private void Awake()
        {
            Application.targetFrameRate = 60;

#if UNITY_EDITOR
            useSaveSystem = true;
#endif
            
            if (useSaveSystem)
            {
                SaveLoadSystem.Init("Saves", "Save", "txt");
                SaveLoadSystem.LoadProgress();
            }

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
                    SaveData.UnlimitedPackagePurchasedNumber++;
                }
                else if (productName == "한정 패키지")
                {
                    SaveData.LimitedPackagePurchased = true;
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
            SaveProgress();
        }

        public void SaveProgress()
        {
            if (SaveLoadSystem.Validation == SaveLoadSystem.DataValidation.Invalidate)
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