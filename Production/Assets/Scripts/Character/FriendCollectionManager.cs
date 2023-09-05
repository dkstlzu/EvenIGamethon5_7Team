using System;
using System.Collections.Generic;
using dkstlzu.Utility;
using MoonBunny.Dev;
using UnityEngine;

namespace MoonBunny
{
    public class FriendCollectionManager : Singleton<FriendCollectionManager>
    {
        private FriendCollection _collectionData;
        public FriendCollection Collection;
        
        public List<FriendCollection.Data> CollectingFriendCharacterList;
        public List<FriendCollection.Data> CollectedFriendCharacterList;

        public int TotalCollectionNumber
        {
            get
            {
                int total = 0;

                foreach (FriendCollection.Data data in CollectedFriendCharacterList)
                {
                    total += data.TargetCollectingNumber;
                }
                
                foreach (FriendCollection.Data data in CollectingFriendCharacterList)
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

                foreach (FriendCollection.Data data in CollectedFriendCharacterList)
                {
                    total += data.CurrentCollectingNumber;
                }
                
                foreach (FriendCollection.Data data in CollectingFriendCharacterList)
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
                return GetData(name);
            }
            set
            {
                FriendCollection.Data data = GetData(name);
                data.TargetCollectingNumber = value.TargetCollectingNumber;
                data.CurrentCollectingNumber = value.CurrentCollectingNumber;
            }
        }

        private void Awake()
        {
            _collectionData = new FriendCollection();
            Collection.CopyTo(_collectionData);
            
            _gameManager = GameManager.instance;
            _gameManager.SaveLoadSystem.OnSaveDataLoaded += () =>
            {
                MoonBunnyLog.print($"FriendCollection Save Loaded");

                foreach (FriendCollection.Data data in _collectionData.Datas)
                {
                    data.CurrentCollectingNumber = GameManager.SaveData.CollectionDict[data.Name];
                    if (data.TargetCollectingNumber <= data.CurrentCollectingNumber)
                    {
                        CollectedFriendCharacterList.Add(data);
                    }
                    else
                    {
                        CollectingFriendCharacterList.Add(data);
                    }
                }
            };
        }

        public void Collect(FriendName name, int number)
        {
            FriendCollection.Data data = GetCollectingData(name);
            if (data == null)
            {
                MoonBunnyLog.print($"{name} friend is already collected so can not collected anymore");
                return;
            }

            int collectedNumber = Mathf.Clamp(data.CurrentCollectingNumber + number, 0, data.TargetCollectingNumber);
            
            GetData(name).CurrentCollectingNumber = collectedNumber;

            GameManager.SaveData.CollectionDict[name] = collectedNumber;
            
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