using System;
using System.Collections.Generic;
using dkstlzu.Utility;
using MoonBunny.Dev;
using MoonBunny.UIs;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MoonBunny
{
    [DefaultExecutionOrder(-1)]
    public class GameManager : Singleton<GameManager>
    {
        public SceneLoadCallbackSetter SCB;
        public SaveLoadSystem SaveLoadSystem;

        public Stage Stage;

        private int _score;
        public int Score
        {
            get => _score;
            set
            {
                _score = value;
                Stage?.UI.SetScore(_score);
            }
        }
        public Transform StartPosition;
        [HideInInspector] public float GlobalGravity;
        [HideInInspector] public bool Stage1Clear;
        [HideInInspector] public bool Stage2Clear;
        [HideInInspector] public bool Stage3Clear;
        [HideInInspector] public bool Stage4Clear;
        [HideInInspector] public bool Stage5Clear;

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

            OnStageSceneLoaded += () => FindDefaultStartPosition();
            OnStageSceneLoaded += () => FindStageObject();
        }

        private void Start()
        {
            Item.OnItemTaken += OnItemTaken;
        }

        private void OnDestroy()
        {
            Item.OnItemTaken -= OnItemTaken;
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
            
            Stage1Clear = SaveLoadSystem.SaveData.Stage1Clear;
            Stage2Clear = SaveLoadSystem.SaveData.Stage2Clear;
            Stage3Clear = SaveLoadSystem.SaveData.Stage3Clear;
            Stage4Clear = SaveLoadSystem.SaveData.Stage4Clear;
            Stage5Clear = SaveLoadSystem.SaveData.Stage5Clear;
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

            SaveLoadSystem.SaveData.Stage1Clear = Stage1Clear;
            SaveLoadSystem.SaveData.Stage2Clear = Stage2Clear;
            SaveLoadSystem.SaveData.Stage3Clear = Stage3Clear;
            SaveLoadSystem.SaveData.Stage4Clear = Stage4Clear;
            SaveLoadSystem.SaveData.Stage5Clear = Stage5Clear;

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
        
        private void OnItemTaken(Item item)
        {
            Score += item.Score;
        }

        void FindDefaultStartPosition()
        {
            CoroutineHelper.OnNextFrame(() =>
            {
                StartPosition = GameObject.FindWithTag("DefaultStartPosition").transform;
            });
        }

        void FindStageObject()
        {
            CoroutineHelper.OnNextFrame(() =>
            {
                Stage = GameObject.FindWithTag("Level").GetComponent<Stage>();
            });
        }

        public void GameOver()
        {
            MoonBunnyLog.print("GameOver");
            MoonBunnyRigidbody.DisableAll();
            Stage.UI.Fail();
        }

        public void RestartApplication()
        {
            SceneManager.LoadScene(SceneName.Loading);
        }
        
        
    }
}