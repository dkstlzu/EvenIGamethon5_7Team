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
        public SceneLoadCallbackSetter SCB;
        public SaveLoadSystem SaveLoadSystem;

        public Stage Stage;

        // private int _score;
        // public int Score
        // {
        //     get => _score;
        //     set
        //     {
        //         _score = value;
        //         Stage?.UI.SetScore(_score);
        //     }
        // }
        public Transform StartPosition;
        [HideInInspector] public float GlobalGravity;
        [HideInInspector] public bool Stage1_1Clear;
        [HideInInspector] public bool Stage1_2Clear;
        [HideInInspector] public bool Stage1_3Clear;
        [HideInInspector] public bool Stage2_1Clear;
        [HideInInspector] public bool Stage2_2Clear;
        [HideInInspector] public bool Stage2_3Clear;
        [HideInInspector] public bool Stage3_1Clear;
        [HideInInspector] public bool Stage3_2Clear;
        [HideInInspector] public bool Stage3_3Clear;
        [HideInInspector] public bool Stage4_1Clear;
        [HideInInspector] public bool Stage4_2Clear;
        [HideInInspector] public bool Stage4_3Clear;
        [HideInInspector] public bool Stage5_1Clear;
        [HideInInspector] public bool Stage5_2Clear;
        [HideInInspector] public bool Stage5_3Clear;

        public StartSceneUI StartSceneUI;
        private int _diamondNumber;
        public int DiamondNumber
        {
            get => _diamondNumber;
            set
            {
                _diamondNumber = value;
                if (StartSceneUI) StartSceneUI.DiamonText1.text = value.ToString();
                if (StartSceneUI) StartSceneUI.DiamonText2.text = value.ToString();
            }
        }

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

        public bool isFirstPlay;
        [SerializeField] private GameObject _tutorialUIGO;

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
                SaveLoadSystem.LoadDatabase();
                LoadProgress();
#if UNITY_EDITOR
            }
#endif

            isFirstPlay = PlayerPrefs.GetInt("MoonBunnyFirstPlay", 1) > 0 ? true : false;

            SCB = new SceneLoadCallbackSetter(SceneName.Names);

            if (isFirstPlay)
            {
                SCB.SceneLoadCallBackDict[SceneName.Stage1_1] += FirstPlayTutorialOn;
            }

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
            if (!SaveLoadSystem.DataIsLoaded) SaveLoadSystem.LoadDatabase();

            GoldNumber = SaveLoadSystem.SaveData.GoldNumber;
            
            Stage1_1Clear = SaveLoadSystem.SaveData.Stage1_1Clear;
            Stage1_2Clear = SaveLoadSystem.SaveData.Stage1_2Clear;
            Stage1_3Clear = SaveLoadSystem.SaveData.Stage1_3Clear;
            Stage2_1Clear = SaveLoadSystem.SaveData.Stage2_1Clear;
            Stage2_2Clear = SaveLoadSystem.SaveData.Stage2_2Clear;
            Stage2_3Clear = SaveLoadSystem.SaveData.Stage2_3Clear;
            Stage3_1Clear = SaveLoadSystem.SaveData.Stage3_1Clear;
            Stage3_2Clear = SaveLoadSystem.SaveData.Stage3_2Clear;
            Stage3_3Clear = SaveLoadSystem.SaveData.Stage3_3Clear;
            Stage4_1Clear = SaveLoadSystem.SaveData.Stage4_1Clear;
            Stage4_2Clear = SaveLoadSystem.SaveData.Stage4_2Clear;
            Stage4_3Clear = SaveLoadSystem.SaveData.Stage4_3Clear;
            Stage5_1Clear = SaveLoadSystem.SaveData.Stage5_1Clear;
            Stage5_2Clear = SaveLoadSystem.SaveData.Stage5_2Clear;
            Stage5_3Clear = SaveLoadSystem.SaveData.Stage5_3Clear;
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

            SaveLoadSystem.SaveData.GoldNumber = GoldNumber;

            SaveLoadSystem.SaveData.Stage1_1Clear = Stage1_1Clear;
            SaveLoadSystem.SaveData.Stage1_2Clear = Stage1_2Clear;
            SaveLoadSystem.SaveData.Stage1_3Clear = Stage1_3Clear;
            SaveLoadSystem.SaveData.Stage2_1Clear = Stage2_1Clear;
            SaveLoadSystem.SaveData.Stage2_2Clear = Stage2_2Clear;
            SaveLoadSystem.SaveData.Stage2_3Clear = Stage2_3Clear;
            SaveLoadSystem.SaveData.Stage3_1Clear = Stage3_1Clear;
            SaveLoadSystem.SaveData.Stage3_2Clear = Stage3_2Clear;
            SaveLoadSystem.SaveData.Stage3_3Clear = Stage3_3Clear;
            SaveLoadSystem.SaveData.Stage4_1Clear = Stage4_1Clear;
            SaveLoadSystem.SaveData.Stage4_2Clear = Stage4_2Clear;
            SaveLoadSystem.SaveData.Stage4_3Clear = Stage4_3Clear;
            SaveLoadSystem.SaveData.Stage5_1Clear = Stage5_1Clear;
            SaveLoadSystem.SaveData.Stage5_2Clear = Stage5_2Clear;
            SaveLoadSystem.SaveData.Stage5_3Clear = Stage5_3Clear;

            SaveLoadSystem.SaveDatabase();
        }

        public void LoadCollection(FriendCollectionManager manager)
        {
#if UNITY_EDITOR
            if (!useSaveSystem) return;
#endif
            FriendCollection.Data data = manager[FriendName.First];
            data.CurrentCollectingNumber = SaveLoadSystem.SaveData.FirstFriendCollectedNumber;
            data = manager[FriendName.Second];
            data.CurrentCollectingNumber = SaveLoadSystem.SaveData.SecondFriendCollectedNumber;
            data = manager[FriendName.Third];
            data.CurrentCollectingNumber = SaveLoadSystem.SaveData.ThirdFriendCollectedNumber;
        }

        public void SaveCollection(FriendCollectionManager manager)
        {
#if UNITY_EDITOR
            if (!useSaveSystem) return;
#endif
            SaveLoadSystem.SaveData.FirstFriendCollectedNumber = manager[FriendName.First].CurrentCollectingNumber;
            SaveLoadSystem.SaveData.SecondFriendCollectedNumber = manager[FriendName.Second].CurrentCollectingNumber;
            SaveLoadSystem.SaveData.ThirdFriendCollectedNumber = manager[FriendName.Third].CurrentCollectingNumber;
        }

        public void FirstPlayTutorialOn()
        {
            Instantiate(_tutorialUIGO).GetComponent<TutorialUI>().On();
            SCB.SceneLoadCallBackDict[SceneName.Stage1_1] -= FirstPlayTutorialOn;
            PlayerPrefs.SetInt("MoonBunnyFirstPlay", 0);
        }

        public void RestartApplication()
        {
            SceneManager.LoadScene(SceneName.Loading);
        }
        
        
    }
}