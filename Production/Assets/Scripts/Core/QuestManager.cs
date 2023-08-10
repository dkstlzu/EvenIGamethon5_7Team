using System;
using System.Collections.Generic;
using System.Linq;
using dkstlzu.Utility;
using MoonBunny.UIs;
using UnityEngine;

namespace MoonBunny
{
    public class QuestManager : Singleton<QuestManager>
    {
        public SaveLoadSystem SaveLoadSystem;
        [SerializeField] private ReadOnlyWithClassDict<int, Quest> _questDict = new ReadOnlyWithClassDict<int, Quest>();

#if UNITY_EDITOR
        public bool UseSaveSystem;
#endif
        
        public QuestSaveData SaveData
        {
            get
            {
                if (!instance.SaveLoadSystem.DataIsLoaded) return null;
                else return instance.SaveLoadSystem.QuestSaveData;
            }
        }

        private const string QUEST_PATH = "Specs/Quest/";

        public const int JUMP_ID = 0;
        public const int CHANGE_DIRECTION_ID = 1;
        public const int SIDEWALL_COLLISION_ID = 2;
        public const int ITEM_TAKEN_ID = 3;
        public const int OBSTACLE_COLLISION_ID = 4;
        public const int GOLD_GET_ID = 5;
        public const int DIAMOND_GET_ID = 6;
        
        public const int UNLOCK_STAGE_ID = 100;
        public const int SUGAR_PERFECTCLEAR_ID = 110;
        public const int FRIEND_COLLECTION_ID = 130;
        public const int PERFECTCLEAR_ID = 140;
        
        void SpecialQuestSet(Quest quest)
        {
            switch (quest.Id)
            {

            }
        }
        
        private void Awake()
        {
            SetByDefaultSpec();
            SetDependentQuests();
            LoadQuestData();
            SetQuestProgressCheckers();
        }
        
        #region Initialize
        void SetByDefaultSpec()
        {
            QuestSpec[] specs = Resources.LoadAll<QuestSpec>(QUEST_PATH);

            foreach (QuestSpec spec in specs)
            {
                Quest quest = new Quest
                {
                    Id = spec.Id,
                    TargetProgress = spec.TargetProgress,
                    DependentId = spec.DependentId,
                    Repeatable = spec.Repeatable,
                    DescriptionText = spec.DescriptionText,
                    DescriptionTextOnDisabled = spec.DescriptionTextOnDisabled,
                    DescriptionTextOnHidden = spec.DescriptionTextOnHidden,
                    Reward = spec.Reward,
                };

                SpecialQuestSet(quest);

                _questDict.Add(spec.Id, quest);
            }
        }

        void SetDependentQuests()
        {
            foreach (var pair in _questDict)
            {
                if (pair.Value.DependentId >= 0)
                {
                    pair.Value.State = QuestState.Disabled;
                    _questDict[pair.Value.DependentId].OnStateChanged += (state) =>
                    {
                        if (state == QuestState.IsFinished)
                        {
                            if (pair.Value.CurrentProgress >= pair.Value.TargetProgress)
                            {
                                pair.Value.State = QuestState.CanTakeReward;
                            }
                            else
                            {
                                pair.Value.State = QuestState.Enabled;
                            }
                        }
                    };
                }
            }
        }

        void LoadQuestData()
        {
#if UNITY_EDITOR
            if (UseSaveSystem)
            {
#endif
                SaveLoadSystem.Init("Saves", "Quest", "txt");

                SaveLoadSystem.OnSaveDataLoaded += () =>
                {
                    for (int i = 0; i < SaveLoadSystem.QuestSaveData.QuestClearList.Count; i++)
                    {
                        QuestItemSaveData questItemSaveData = SaveLoadSystem.QuestSaveData.QuestClearList[i];

                        Quest targetQuest = _questDict[questItemSaveData.Id];

                        targetQuest.CurrentProgress = questItemSaveData.CurrentProgress;
                        targetQuest.ItemSaveData = questItemSaveData;

                        if (targetQuest.State == QuestState.Disabled)
                        {
                        }
                        else if (questItemSaveData.isFinished)
                        {
                            targetQuest.State = QuestState.IsFinished;
                        }
                        else if (targetQuest.CurrentProgress >= targetQuest.TargetProgress)
                        {
                            targetQuest.State = QuestState.CanTakeReward;
                        }
                    }
                };

                SaveLoadSystem.LoadQuest();
#if UNITY_EDITOR
            }
            else
            {
                SaveLoadSystem.DataIsLoaded = true;
                SaveLoadSystem.QuestSaveData = QuestSaveData.GetDefaultSaveData();
            }
#endif
        }
        

        #endregion

        #region QuestProgress

        public int ChangeDirectionNumber;

        void SetQuestProgressCheckers()
        {
            GameManager.instance.OnStageSceneLoaded += () =>
            {
                GameManager.instance.Stage.UI.OnDirectionChangeButtonClicked += OnChangeDirection;
            };
            GameManager.instance.OnStageSceneUnloaded += () =>
            {
                SaveLoadSystem.SaveQuest();
            };
            
            FriendCollectionManager.instance.OnCollectFriendFinish += OnCollectFinish;
            StoreUI.OnGotchaRewardGet += OnGotchaReward;
            Item.OnInvoke += OnItemInvoke;
            Obstacle.OnInvoke += OnObstacleInvoke;
            BouncyPlatform.OnInvoke += OnJump;
            SideWall.OnPlayerCollide += OnSideWallCollision;
            Stage.OnNewStageUnlocked += OnNewStageUnlocked;
            Stage.OnNewLevelUnlocked += OnNewLevelUnlocked;
            Stage.OnStageClear += OnStageClear;
        }

        private void OnGotchaReward(GotchaReward reward)
        {
            GetGold(reward.GoldNumber, false);
            GetDiamond(reward.DiamondNumber, false);
        }

        private void OnSideWallCollision()
        {
            SaveData.SideWallCollisionCount++;
            _questDict[SIDEWALL_COLLISION_ID].ProgressAhead();
        }

        private void OnJump()
        {
            SaveData.JumpCount++;
            _questDict[JUMP_ID].ProgressAhead();
        }

        private void OnChangeDirection()
        {
            SaveData.ChangeDirectionCount++;
            _questDict[CHANGE_DIRECTION_ID].ProgressAhead();
        }
        
        private void OnCollectFinish(FriendName friendName)
        {
            int targetID = FRIEND_COLLECTION_ID + (int)friendName - 1;
            _questDict[targetID].ProgressAhead();
        }

        private void OnItemInvoke(Item item)
        {
            SaveData.ItemTakenCount++;
            if (item is Coin)
            {
                GetGold(1);
            }
            else
            {
                _questDict[ITEM_TAKEN_ID].ProgressAhead();
            }
        }

        private void OnObstacleInvoke()
        {
            SaveData.ObstacleCollisionCount++;
            _questDict[OBSTACLE_COLLISION_ID].ProgressAhead();
        }

        private void OnNewStageUnlocked(int stageLevel)
        {
            int targetId = UNLOCK_STAGE_ID + stageLevel - 1;
            _questDict[targetId].ProgressAhead();
        }
        
        private void OnNewLevelUnlocked(int subLevel)
        {
        }
        
        private void OnStageClear(Stage stage)
        {
            if (stage.GainedStar == 3 && GameManager.instance.UsingFriendName == FriendName.Sugar)
            {
                int targetId = SUGAR_PERFECTCLEAR_ID + stage.StageLevel * 3 + stage.SubLevel;
                _questDict[targetId].ProgressAhead();                
            } else if (stage.StageLevel == (int)StageName.StarfulMilkyWay)
            {
                int targetId = PERFECTCLEAR_ID + (int)GameManager.instance.UsingFriendName - 1;
                _questDict[targetId].ProgressAhead();
            }
        }

        #endregion
        
        public void GetDiamond(int number, bool onlyQuest = true)
        {
            SaveData.DiamondGetCount += number;
            _questDict[DIAMOND_GET_ID].ProgressAhead(number);
            if (!onlyQuest)
            {
                GameManager.instance.DiamondNumber += number;
            }
        }

        public void GetGold(int number, bool onlyQuest = true)
        {
            SaveData.GoldGetCount += number;
            _questDict[GOLD_GET_ID].ProgressAhead(number);
            if (!onlyQuest)
            {
                GameManager.instance.GoldNumber += number;
            }
        }
        
        private void OnApplicationQuit()
        {
            SaveLoadSystem.SaveQuest();
        }
        
        public Quest GetQuest(int id)
        {
            return _questDict[id];
        }

        public Quest[] GetAllQuest()
        {
            return _questDict.Values.ToArray();
        }
    }
}