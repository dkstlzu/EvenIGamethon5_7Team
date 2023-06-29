using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MoonBunny
{
    public class GameManager : Singleton<GameManager>
    {
        public SaveLoadSystem SaveLoadSystem;
        public int Score;
        public Transform StartPosition;
        [HideInInspector] public float GlobalGravity;
        [HideInInspector] public bool Stage1Clear;
        [HideInInspector] public bool Stage2Clear;
        [HideInInspector] public bool Stage3Clear;
        [HideInInspector] public bool Stage4Clear;
        [HideInInspector] public bool Stage5Clear;
        
#if UNITY_EDITOR
        public bool useSaveSystem;
#endif

        ~GameManager()
        {
            SaveLoadSystem.Dispose();    
        }
        
        private void Awake()
        {
#if UNITY_EDITOR
            if (useSaveSystem)
            {
#endif
                SaveLoadSystem = new SaveLoadSystem();
                LoadProgress();
#if UNITY_EDITOR
            }
#endif
        }

        private void Start()
        {
            Item.OnItemTaken += OnItemTaken;
            SceneManager.sceneLoaded += FindDefaultStartPosition;
        }

        private void OnDestroy()
        {
            Item.OnItemTaken -= OnItemTaken;
        }

        private void OnApplicationQuit()
        {
#if UNITY_EDITOR
            if (useSaveSystem)
            {
#endif
                SaveProgress();
                SaveLoadSystem.Dispose();
#if UNITY_EDITOR
            }
#endif
        }

        public void LoadProgress()
        {
            SaveLoadSystem.LoadDatabase();

            FriendCollection.Data data = FriendCollectionManager.instance[FriendName.First];
            data.CurrentCollectingNumber = SaveLoadSystem.SaveData.FirstFriendCollectedNumber;
            data = FriendCollectionManager.instance[FriendName.Second];
            data.CurrentCollectingNumber = SaveLoadSystem.SaveData.SecondFriendCollectedNumber;
            data = FriendCollectionManager.instance[FriendName.Third];
            data.CurrentCollectingNumber = SaveLoadSystem.SaveData.ThirdFriendCollectedNumber;

            Stage1Clear = SaveLoadSystem.SaveData.Stage1Clear;
            Stage2Clear = SaveLoadSystem.SaveData.Stage2Clear;
            Stage3Clear = SaveLoadSystem.SaveData.Stage3Clear;
            Stage4Clear = SaveLoadSystem.SaveData.Stage4Clear;
            Stage5Clear = SaveLoadSystem.SaveData.Stage5Clear;
        }

        public void SaveProgress()
        {
            SaveLoadSystem.SaveData.Stage1Clear = Stage1Clear;
            SaveLoadSystem.SaveData.Stage2Clear = Stage2Clear;
            SaveLoadSystem.SaveData.Stage3Clear = Stage3Clear;
            SaveLoadSystem.SaveData.Stage4Clear = Stage4Clear;
            SaveLoadSystem.SaveData.Stage5Clear = Stage5Clear;
            
            SaveLoadSystem.SaveData.FirstFriendCollectedNumber = FriendCollectionManager.instance[FriendName.First].CurrentCollectingNumber;
            SaveLoadSystem.SaveData.SecondFriendCollectedNumber = FriendCollectionManager.instance[FriendName.Second].CurrentCollectingNumber;
            SaveLoadSystem.SaveData.ThirdFriendCollectedNumber = FriendCollectionManager.instance[FriendName.Third].CurrentCollectingNumber;
            
            SaveLoadSystem.SaveDatabase();
        }

        private void OnItemTaken(Item item)
        {
            Score += item.Score;
        }

        void FindDefaultStartPosition(Scene scene, LoadSceneMode loadSceneMode)
        {
            StartPosition = GameObject.FindWithTag("DefaultStartPosition").transform;
        }

        public void RestartApplication()
        {
            SceneManager.LoadScene(SceneName.Loading);
        }
        
        
    }
}