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
                    return null;
                }
                else return instance.SaveLoadSystem.ProgressSaveData;
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
#if UNITY_EDITOR
            if (useSaveSystem)
            {
#endif
                SaveLoadSystem = new SaveLoadSystem("Saves", "Save", "txt");
                SaveLoadSystem.LoadProgress();
#if UNITY_EDITOR
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

            GoogleManager.instance.OnLoginPlayStoreSuccess += CheckReady;
            SaveLoadSystem.OnSaveDataLoaded += CheckReady;
            QuestManager.instance.SaveLoadSystem.OnSaveDataLoaded += CheckReady;
        }

        private void CheckReady()
        {
            if (GoogleManager.instance.SignInStatus == SignInStatus.Success && SaveLoadSystem.DataIsLoaded &&
                QuestManager.instance.SaveLoadSystem.DataIsLoaded)
            {
                CoroutineHelper.Delay(() =>
                {
                    SceneManager.LoadSceneAsync(SceneName.Scenario);
                }, 3f);
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
            
            SaveLoadSystem.SaveProgress();
        }

        public void RestartApplication()
        {
            SceneManager.LoadScene(SceneName.Loading);
        }
    }
}