using System;
using System.Collections.Generic;
using dkstlzu.Utility;
using UnityEngine;
using UnityEngine.Serialization;

namespace MoonBunny
{
    [Serializable]
    public class LegacyProgressSaveData
    {
        [SerializeField] private ReadOnlyEnumDict<FriendName, int> _collectionDict = new ReadOnlyEnumDict<FriendName, int>();
        [SerializeField] private ReadOnlyEnumDict<FriendName, int> _collectionSellDict = new ReadOnlyEnumDict<FriendName, int>();
        [SerializeField] private ReadOnlyEnumDict<StageName, int> _clearDict = new ReadOnlyEnumDict<StageName, int>();

        public ReadOnlyEnumDict<FriendName, int> CollectionDict => _collectionDict;
        public ReadOnlyEnumDict<FriendName, int> CollectionSellDict => _collectionSellDict;
        public ReadOnlyEnumDict<StageName, int> ClearDict => _clearDict;

        public string UsingFriendName;
        public int DiamondNumber;
        public int GoldNumber;

        public bool ShowTutorial;
        public float VolumeSetting;
    }
    
    
    [Serializable]
    public class ProgressSaveData
    {
        [SerializeField] private ReadOnlyEnumDict<FriendName, int> _collectionDict = new ReadOnlyEnumDict<FriendName, int>();
        [SerializeField] private ReadOnlyEnumDict<FriendName, int> _collectionSellDict = new ReadOnlyEnumDict<FriendName, int>();
        [SerializeField] private ReadOnlyEnumDict<StageName, int> _clearDict = new ReadOnlyEnumDict<StageName, int>();

        public ReadOnlyEnumDict<FriendName, int> CollectionDict => _collectionDict;
        public ReadOnlyEnumDict<FriendName, int> CollectionSellDict => _collectionSellDict;
        public ReadOnlyEnumDict<StageName, int> ClearDict => _clearDict;

        public string UsingFriendName;
        public int DiamondNumber;
        public int GoldNumber;

        public bool ShowTutorial;
        public float VolumeSetting;
        
        public bool RemoveAd;
        public bool LimitedPackagePurchased;
        public int UnlimitedPackagePurchasedNumber;

        public static ProgressSaveData GetDefaultSaveData()
        {
            ProgressSaveData progressSaveData = new ProgressSaveData();
                
            foreach (FriendName friendName in (FriendName[])Enum.GetValues(typeof(FriendName)))
            {
                if ((int)friendName < 0) continue;
                progressSaveData.CollectionDict.Add(friendName, 0);
                progressSaveData.CollectionSellDict.Add(friendName, 0);
            }

            foreach (StageName stageName in (StageName[])Enum.GetValues(typeof(StageName)))
            {
                if ((int)stageName < 0) continue;
                progressSaveData.ClearDict.Add(stageName, -1);
            }

            progressSaveData.ClearDict[StageName.GrassField] = 0;
            
            progressSaveData.UsingFriendName = FriendName.Sugar.ToString();
            progressSaveData.DiamondNumber = 0;
            progressSaveData.GoldNumber = 0;

            progressSaveData.ShowTutorial = true;
            progressSaveData.VolumeSetting = 1;

            return progressSaveData;
        }
        
        private const string CollectionAssetPath = "Assets/Resources/Specs/FriendCollection.asset"; 

        public static ProgressSaveData GetFullSaveData()
        {
            ProgressSaveData data = GetDefaultSaveData();

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

            data.DiamondNumber = 9999;
            data.GoldNumber = 9999;
            
            data.ShowTutorial = false;

            return data;
        }

        public static ProgressSaveData MigrateFromLegacy(LegacyProgressSaveData data)
        {
            ProgressSaveData newData = GetDefaultSaveData();

            foreach (var pair in data.CollectionDict)
            {
                if (newData.CollectionDict.ContainsKey(pair.Key))
                {
                    newData.CollectionDict[pair.Key] = pair.Value;
                }
            }
            
            foreach (var pair in data.CollectionSellDict)
            {
                if (newData.CollectionSellDict.ContainsKey(pair.Key))
                {
                    newData.CollectionSellDict[pair.Key] = pair.Value;
                }
            }

            foreach (var pair in data.ClearDict)
            {
                if (newData.ClearDict.ContainsKey(pair.Key))
                {
                    newData.ClearDict[pair.Key] = pair.Value;
                }
            }

            newData.UsingFriendName = data.UsingFriendName;
            newData.DiamondNumber = data.DiamondNumber;
            newData.GoldNumber = data.GoldNumber;

            newData.ShowTutorial = data.ShowTutorial;
            newData.VolumeSetting = data.VolumeSetting;
            
            return newData;
        }
    }
}