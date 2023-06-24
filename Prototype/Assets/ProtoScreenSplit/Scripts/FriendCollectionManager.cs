using System;
using System.Collections.Generic;
using UnityEngine;

namespace EvenI7.ProtoScreenSplit
{
    public class FriendCollectionManager : Singleton<FriendCollectionManager>
    {
        public Dictionary<FriendName, FriendCollection> CollectionDict;
        
        public List<FriendCollection> CollectingFriendCharacterList;
        public List<FriendCollection> CollectedFriendCharacterList;

        private void Awake()
        {
            CollectionDict = new Dictionary<FriendName, FriendCollection>();
            CollectionDict.Add(FriendName.First, CollectingFriendCharacterList[0]);
            CollectionDict.Add(FriendName.Second, CollectingFriendCharacterList[1]);
            CollectionDict.Add(FriendName.Third, CollectingFriendCharacterList[2]);
        }

        public int GetTargetNumber(FriendName name)
        {
            return CollectionDict[name].TargetCollectingNumber;
        }

        public int GetCurrentNumber(FriendName name)
        {
            return CollectionDict[name].CurrentCollectingNumber;
        }

        public void Collect(FriendName name)
        {
            var coll = CollectionDict[name];
            coll.CurrentCollectingNumber++;
            
            if (coll.CurrentCollectingNumber >= coll.TargetCollectingNumber)
            {
                CollectFinish(name);    
            }
        }

        public void CollectFinish(FriendName name)
        {
            CollectingFriendCharacterList.Remove(CollectionDict[name]);
            CollectedFriendCharacterList.Add(CollectionDict[name]);
        }
    }

    [Serializable]
    public class FriendCollection
    {
        public FriendName Name;
        public int TargetCollectingNumber;
        public int CurrentCollectingNumber;
    }
}