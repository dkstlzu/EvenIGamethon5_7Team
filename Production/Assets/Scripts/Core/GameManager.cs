﻿using System;
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

        
        public SceneLoadCallbackSetter SCB;
        public SaveLoadSystem SaveLoadSystem;

        public Stage Stage;
        public FriendName UsingFriendName;

        public ReadOnlyEnumDict<FriendName, int> CollectDict;
        public ReadOnlyEnumDict<FriendName, int> CollectSellDict;
        public ReadOnlyEnumDict<StageName, int> ClearDict;
        public ReadOnlyEnumDict<QuestType, bool> QuestClearDict;

        public StartSceneUI StartSceneUI;
        

        private int _goldNumber;

        public int GoldNumber
        {
            get => _goldNumber;
            set
            {
                _goldNumber = value;
                if (StartSceneUI) StartSceneUI.GoldText1.text = value.ToString();
                if (StartSceneUI) StartSceneUI.GoldText2.text = value.ToString();
            }
        }

        public bool ShowTutorial;
        [SerializeField] private float _volumeSetting;
        public float VolumeSetting
        {
            get => _volumeSetting;
            set
            {
                _volumeSetting = value;
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
                SaveLoadSystem = new SaveLoadSystem();
                SaveLoadSystem.OnSaveDataLoaded += LoadProgress;
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

        public void LoadProgress()
        {
#if UNITY_EDITOR
            if (!useSaveSystem) return;
#endif
            CollectDict = SaveLoadSystem.SaveData.CollectionDict;
            CollectSellDict = SaveLoadSystem.SaveData.CollectionSellDict;
            ClearDict = SaveLoadSystem.SaveData.ClearDict;
            QuestClearDict = SaveLoadSystem.SaveData.QuestClearDict;

            UsingFriendName = Enum.Parse<FriendName>(SaveLoadSystem.SaveData.UsingFriendName);
            GoldNumber = SaveLoadSystem.SaveData.GoldNumber;

            ShowTutorial = SaveLoadSystem.SaveData.ShowTutorial;
            VolumeSetting = SaveLoadSystem.SaveData.VolumeSetting;
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
            
            SaveLoadSystem.SaveData.UsingFriendName = UsingFriendName.ToString();
            SaveLoadSystem.SaveData.GoldNumber = GoldNumber;

            SaveLoadSystem.SaveData.ShowTutorial = ShowTutorial;
            SaveLoadSystem.SaveData.VolumeSetting = VolumeSetting;

            SaveLoadSystem.SaveDatabase();
        }

        public void SaveCollection()
        {
#if UNITY_EDITOR
            if (!useSaveSystem) return;
#endif
            foreach (var data in FriendCollectionManager.instance.Collection.Datas)
            {
                SaveLoadSystem.SaveData.CollectionDict[data.Name] = data.CurrentCollectingNumber;
            }
            
            SaveLoadSystem.SaveDatabase();
        }

        public void RestartApplication()
        {
            SceneManager.LoadScene(SceneName.Loading);
        }
    }
}