using System;
using System.Collections.Generic;
using UnityEngine;

namespace MoonBunny
{
    public class FriendCollectionManager : Singleton<FriendCollectionManager>
    {
        public FriendCollection Collection;
        
        public List<FriendCollection.Data> CollectingFriendCharacterList;
        public List<FriendCollection.Data> CollectedFriendCharacterList;

        public event Action<FriendName, int> OnCollectFriend;
        public event Action<FriendName> OnCollectFriendFinish;

        public FriendCollection.Data this[FriendName name]
        {
            get
            {
                foreach (var data in Collection.Datas)
                {
                    if (data.Name == name) return data;
                }

                return null;
            }
            set
            {
                foreach (var data in Collection.Datas)
                {
                    if (data.Name == name)
                    {
                        data.TargetCollectingNumber = value.TargetCollectingNumber;
                        data.CurrentCollectingNumber = value.CurrentCollectingNumber;
                    }
                }
            }
        }

        private void Awake()
        {
            foreach (var collection in Collection.Datas)
            {
                if (collection.TargetCollectingNumber <= collection.CurrentCollectingNumber)
                {
                    CollectedFriendCharacterList.Add(collection);
                }
                else
                {
                    CollectingFriendCharacterList.Add(collection);
                }
            }
        }
        
        public void Collect(FriendName name)
        {
            if (CollectFinished(name))
            {
                return;
            }
            
            FriendCollection.Data data = GetCollectingData(name);
            data.CurrentCollectingNumber += 1;
            
            OnCollectFriend?.Invoke(name, data.CurrentCollectingNumber);
            if (data.Finish()) CollectFinish(name);
        }

        public void CollectFinish(FriendName name)
        {
            FriendCollection.Data data = GetCollectingData(name);
            
            CollectingFriendCharacterList.Remove(data);
            CollectedFriendCharacterList.Add(data);
            
            OnCollectFriendFinish?.Invoke(name);
        }

        public bool CollectFinished(FriendName name)
        {
            FriendCollection.Data data = new FriendCollection.Data() { Name = name };

            if (CollectedFriendCharacterList.Contains(data)) return true;
            else return false;
        }

        private FriendCollection.Data GetData(FriendName name)
        {
            foreach (var data in CollectingFriendCharacterList)
            {
                if (data.Name == name) return data;
            }

            foreach (var data in CollectedFriendCharacterList)
            {
                if (data.Name == name) return data;
            }
            
            return null;
        }

        private FriendCollection.Data GetCollectingData(FriendName name)
        {
            foreach (var data in CollectingFriendCharacterList)
            {
                if (data.Name == name) return data;
            }

            return null;
        }

        private FriendCollection.Data GetCollectedData(FriendName name)
        {
            foreach (var data in CollectedFriendCharacterList)
            {
                if (data.Name == name) return data;
            }
            
            return null;
        }
    }
}