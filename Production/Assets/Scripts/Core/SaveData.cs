using System;
using UnityEngine;

namespace MoonBunny
{
    [Serializable]
    public class FriendNameCollectDictionary : SerializableDictionary<FriendName, int>
    {

    }

    [Serializable]
    public class StageNameClearDictionary : SerializableDictionary<StageName, int>
    {
    }
    
    [Serializable]
    public class SaveData
    {
        public FriendNameCollectDictionary CollectionDict = new FriendNameCollectDictionary();
        public StageNameClearDictionary ClearDict = new StageNameClearDictionary();

        public int GoldNumber;

        public SaveData()
        {
            foreach (FriendName friendName in (FriendName[])Enum.GetValues(typeof(FriendName)))
            {
                CollectionDict.Add(friendName, 0);
            }

            foreach (StageName stageName in (StageName[])Enum.GetValues(typeof(StageName)))
            {
                ClearDict.Add(stageName, 0);
            }

            GoldNumber = 0;
        }
    }
}