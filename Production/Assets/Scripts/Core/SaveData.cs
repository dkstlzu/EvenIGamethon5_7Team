using System;
using System.Collections.Generic;
using dkstlzu.Utility;
using UnityEngine;
using UnityEngine.Serialization;

namespace MoonBunny
{
    [Serializable]
    public class SaveData
    {
        [SerializeField] private ReadOnlyEnumDict<FriendName, int> _collectionDict = new ReadOnlyEnumDict<FriendName, int>();
        [SerializeField] private ReadOnlyEnumDict<FriendName, int> _collectionSellDict = new ReadOnlyEnumDict<FriendName, int>();
        [SerializeField] private ReadOnlyEnumDict<StageName, int> _clearDict = new ReadOnlyEnumDict<StageName, int>();
        [SerializeField] private List<QuestSaveData> _questClearList = new List<QuestSaveData>();

        public ReadOnlyEnumDict<FriendName, int> CollectionDict => _collectionDict;
        public ReadOnlyEnumDict<FriendName, int> CollectionSellDict => _collectionSellDict;
        public ReadOnlyEnumDict<StageName, int> ClearDict => _clearDict;
        public List<QuestSaveData> QuestClearList => _questClearList;

        public string UsingFriendName;
        public int GoldNumber;

        public bool ShowTutorial;
        public float VolumeSetting;

        public SaveData()
        {

        }

        public static SaveData GetDefaultSaveData()
        {
            SaveData saveData = new SaveData();
                
            foreach (FriendName friendName in (FriendName[])Enum.GetValues(typeof(FriendName)))
            {
                if ((int)friendName < 0) continue;
                saveData.CollectionDict.Add(friendName, 0);
                saveData.CollectionSellDict.Add(friendName, 0);
            }

            foreach (StageName stageName in (StageName[])Enum.GetValues(typeof(StageName)))
            {
                if ((int)stageName < 0) continue;
                saveData.ClearDict.Add(stageName, 0);
            }
            
            QuestSpec[] questSpecs = Resources.LoadAll<QuestSpec>("Specs/Quest/");

            foreach (QuestSpec spec in questSpecs)
            {
                saveData.QuestClearList.Add(new QuestSaveData(spec.Id));
            }

            saveData.UsingFriendName = FriendName.Sugar.ToString();
            saveData.GoldNumber = 0;

            saveData.ShowTutorial = true;
            saveData.VolumeSetting = 1;

            return saveData;
        }
        
        private const string CollectionAssetPath = "Assets/Resources/Specs/FriendCollection.asset"; 

        public static SaveData GetFullSaveData()
        {
            SaveData data = SaveData.GetDefaultSaveData();

            FriendCollection collection = null;
            
#if UNITY_EDITOR
            if (!UnityEditor.EditorApplication.isPlaying)
            {
                collection = UnityEditor.AssetDatabase.LoadAssetAtPath<FriendCollection>(CollectionAssetPath);
            }
#endif

            if (collection == null)
            {
                collection = Resources.Load<FriendCollection>(CollectionAssetPath);
            }
            
            for (int i = 0; i < data.CollectionDict.Count; i++)
            {
                data.CollectionDict[(FriendName)i] = collection.Datas[i].TargetCollectingNumber;
            }

            for (int i = 0; i < data.ClearDict.Count; i++)
            {
                data.ClearDict[(StageName)i] = 3;
            }

            data.ShowTutorial = false;
            data.GoldNumber = 9999;

            return data;
        }

        public int this[FriendName friendName] => _collectionDict[friendName];
        public int this[StageName stageName] => _clearDict[stageName];
    }
}