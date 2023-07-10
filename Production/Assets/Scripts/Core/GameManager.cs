using System;
using System.Collections.Generic;
using dkstlzu.Utility;
using MoonBunny.UIs;
using UnityEditor.Build.Content;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MoonBunny
{
    [DefaultExecutionOrder(-1)]
    public class GameManager : Singleton<GameManager>
    {
        public SceneLoadCallbackSetter SCB;
        public SaveLoadSystem SaveLoadSystem;
        public int Score;
        public Transform StartPosition;
        [HideInInspector] public float GlobalGravity;
        [HideInInspector] public bool Stage1Clear;
        [HideInInspector] public bool Stage2Clear;
        [HideInInspector] public bool Stage3Clear;
        [HideInInspector] public bool Stage4Clear;
        [HideInInspector] public bool Stage5Clear;

        public bool isFirstPlay;
        [SerializeField] private GameObject _tutorialUIGO;

        public event Action<string> OnStageSceneLoaded;
        public event Action<string> OnStageSceneUnloaded;
        
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

            SCB = new SceneLoadCallbackSetter(SceneName.GetNames());

            if (isFirstPlay)
            {
                SCB.SceneLoadCallBackDict[SceneName.Stage1] += FirstPlayTutorialOn;
            }

            SCB.SceneLoadCallBackDict[SceneName.Stage1] += () => OnStageSceneLoaded?.Invoke(SceneName.Stage1);
            SCB.SceneLoadCallBackDict[SceneName.Stage2] += () => OnStageSceneLoaded?.Invoke(SceneName.Stage2);
            SCB.SceneLoadCallBackDict[SceneName.Stage3] += () => OnStageSceneLoaded?.Invoke(SceneName.Stage3);
            SCB.SceneLoadCallBackDict[SceneName.Stage4] += () => OnStageSceneLoaded?.Invoke(SceneName.Stage4);
            SCB.SceneLoadCallBackDict[SceneName.Stage5] += () => OnStageSceneLoaded?.Invoke(SceneName.Stage5);
            
            SCB.SceneUnloadCallBackDict[SceneName.Stage1] += () => OnStageSceneUnloaded?.Invoke(SceneName.Stage1);
            SCB.SceneUnloadCallBackDict[SceneName.Stage2] += () => OnStageSceneUnloaded?.Invoke(SceneName.Stage2);
            SCB.SceneUnloadCallBackDict[SceneName.Stage3] += () => OnStageSceneUnloaded?.Invoke(SceneName.Stage3);
            SCB.SceneUnloadCallBackDict[SceneName.Stage4] += () => OnStageSceneUnloaded?.Invoke(SceneName.Stage4);
            SCB.SceneUnloadCallBackDict[SceneName.Stage5] += () => OnStageSceneUnloaded?.Invoke(SceneName.Stage5);

            OnStageSceneLoaded += (StageName) => FindDefaultStartPosition();
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
            
            SCB.Dispose();
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

        public void LoadCollection()
        {
#if UNITY_EDITOR
            if (!useSaveSystem) return;
#endif
            FriendCollection.Data data = FriendCollectionManager.instance[FriendName.First];
            data.CurrentCollectingNumber = SaveLoadSystem.SaveData.FirstFriendCollectedNumber;
            data = FriendCollectionManager.instance[FriendName.Second];
            data.CurrentCollectingNumber = SaveLoadSystem.SaveData.SecondFriendCollectedNumber;
            data = FriendCollectionManager.instance[FriendName.Third];
            data.CurrentCollectingNumber = SaveLoadSystem.SaveData.ThirdFriendCollectedNumber;
        }

        public void SaveCollection()
        {
#if UNITY_EDITOR
            if (!useSaveSystem) return;
#endif
            SaveLoadSystem.SaveData.FirstFriendCollectedNumber = FriendCollectionManager.instance[FriendName.First].CurrentCollectingNumber;
            SaveLoadSystem.SaveData.SecondFriendCollectedNumber = FriendCollectionManager.instance[FriendName.Second].CurrentCollectingNumber;
            SaveLoadSystem.SaveData.ThirdFriendCollectedNumber = FriendCollectionManager.instance[FriendName.Third].CurrentCollectingNumber;
        }

        public void FirstPlayTutorialOn()
        {
            Instantiate(_tutorialUIGO).GetComponent<TutorialUI>().On();
            SCB.SceneLoadCallBackDict[SceneName.Stage1] -= FirstPlayTutorialOn;
        }
        
        private void OnItemTaken(Item item)
        {
            Score += item.Score;
        }

        void FindDefaultStartPosition()
        {
            StartPosition = GameObject.FindWithTag("DefaultStartPosition").transform;
        }

        public void RestartApplication()
        {
            SceneManager.LoadScene(SceneName.Loading);
        }
        
        
    }
}