using System;
using System.Collections.Generic;
using dkstlzu.Utility;
using MoonBunny.Dev;
using UnityEngine;

namespace MoonBunny
{
    public class FriendCollectionManager : Singleton<FriendCollectionManager>
    {
        public FriendCollection Collection;
        
        public List<FriendCollection.Data> CollectingFriendCharacterList;
        public List<FriendCollection.Data> CollectedFriendCharacterList;

        public int TotalCollectionNumber
        {
            get
            {
                int total = 0;

                foreach (FriendCollection.Data data in Collection.Datas)
                {
                    total += data.TargetCollectingNumber;
                }

                return total;
            }
        }

        public int CurrentCollectionNumber
        {
            get
            {
                int total = 0;

                foreach (FriendCollection.Data data in Collection.Datas)
                {
                    total += data.CurrentCollectingNumber;
                }

                return total;
            }
        }

        public event Action<FriendName, int> OnCollectFriend;
        public event Action<FriendName> OnCollectFriendFinish;
        
        private GameManager _gameManager;

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
            _gameManager = GameManager.instance;
            _gameManager.SaveLoadSystem.OnSaveDataLoaded += () =>
            {
                foreach (FriendCollection.Data data in Collection.Datas)
                {
                    data.CurrentCollectingNumber = GameManager.ProgressSaveData.CollectionDict[data.Name];
                }
            };
        }

        private void Start()
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

        public void Collect(FriendName name, int number)
        {
            if (CollectFinished(name))
            {
                return;
            }
            
            FriendCollection.Data data = GetCollectingData(name);
            if (data == null)
            {
                MoonBunnyLog.print($"{name} friend is already collected so can not collected anymore");
                return;
            }
            
            data.CurrentCollectingNumber = Mathf.Clamp(data.CurrentCollectingNumber + number, 0, data.TargetCollectingNumber);
            
            _gameManager.SaveLoadSystem.ProgressSaveData.CollectionDict[name] = data.CurrentCollectingNumber;
            
            OnCollectFriend?.Invoke(name, data.CurrentCollectingNumber);
            if (data.IsFinish()) CollectFinish(name);
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

        public FriendName[] GetCollectionFinishedFriendNames()
        {
            List<FriendName> friendNameList = new List<FriendName>();

            foreach (var data in CollectedFriendCharacterList)
            {
                friendNameList.Add(data.Name);
            }

            return friendNameList.ToArray();
        }
    }
}