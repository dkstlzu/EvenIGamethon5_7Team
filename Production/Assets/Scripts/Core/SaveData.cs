using System;
using System.Collections.Generic;
using dkstlzu.Utility;
using UnityEngine;

namespace MoonBunny
{
    [Serializable]
    public class SaveData
    {
        [SerializeField] private ReadOnlyEnumDict<FriendName, int> _collectionDict = new ReadOnlyEnumDict<FriendName, int>();
        [SerializeField] private ReadOnlyEnumDict<StageName, int> _clearDict = new ReadOnlyEnumDict<StageName, int>();
        
        public ReadOnlyEnumDict<FriendName, int> CollectionDict => _collectionDict;
        public ReadOnlyEnumDict<StageName, int> ClearDict => _clearDict;

        public string UsingFriendName;
        public int GoldNumber;

        public SaveData()
        {
            foreach (FriendName friendName in (FriendName[])Enum.GetValues(typeof(FriendName)))
            {
                if ((int)friendName < 0) continue;
                CollectionDict.Add(friendName, 0);
            }
            
            foreach (StageName stageName in (StageName[])Enum.GetValues(typeof(StageName)))
            {
                if ((int)stageName < 0) continue;
                ClearDict.Add(stageName, 0);
            }

            UsingFriendName = FriendName.Sugar.ToString();
            GoldNumber = 0;
        }

        public int this[FriendName friendName] => _collectionDict[friendName];
        public int this[StageName stageName] => _clearDict[stageName];

    }
}