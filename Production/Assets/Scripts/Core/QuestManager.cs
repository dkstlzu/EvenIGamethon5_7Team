using System;
using System.Collections.Generic;
using System.Linq;
using dkstlzu.Utility;
using UnityEngine;

namespace MoonBunny
{
    public class QuestManager : Singleton<QuestManager>
    {
        public SaveLoadSystem SaveLoadSystem;
        [SerializeField] private ReadOnlyWithClassDict<int, Quest> _questDict = new ReadOnlyWithClassDict<int, Quest>();

        private const string QUEST_PATH = "Specs/Quest/";
        private static int[] BouncyPlatformJumpCountQuestIds = {}; 
        private static int[] ChangeDirectionCountQuestIds = {}; 
        private static int[] ItemIvokeCountQuestIds = {}; 
        private static int[] ObstacleInvokeCountQuestIds = {}; 
        private static int[] CoinTakenCountQuestIds = {}; 
        private static int[] DiamondTakenCountQuestIds = {}; 
        private static int[] CharacterCollectionQuestIds = {}; 
        private static int[] NewLevelUnlocckedQuestIds = {}; 
        private static int[] StageClearQuestIds = {}; 
        private static int[] StagePerfectClearQuestIds = {}; 
        

        private void Awake()
        {
            void SpecialQuestSet(Quest quest)
            {
                switch (quest.Id)
                {

                }
            }

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

            foreach (var pair in _questDict)
            {
                if (pair.Value.DependentId >= 0)
                {
                    pair.Value.State = QuestState.Disabled;
                    _questDict[pair.Value.DependentId].OnStateChanged += (state) =>
                    {
                        if (state == QuestState.IsFinished)
                        {
                            pair.Value.State = QuestState.Enabled;
                        }
                    };
                }
            }

            SaveLoadSystem = new SaveLoadSystem("Saves", "Quest", "txt");

            SaveLoadSystem.OnSaveDataLoaded += () =>
            {
                for (int i = 0; i < SaveLoadSystem.QuestSaveData.QuestClearList.Count; i++)
                {
                    QuestItemSaveData questItemSaveData = SaveLoadSystem.QuestSaveData.QuestClearList[i];

                    Quest targetQuest = _questDict[questItemSaveData.Id];
                    
                    targetQuest.CurrentProgress = questItemSaveData.CurrentProgress;
                    targetQuest.ItemSaveData = questItemSaveData;

                    if (questItemSaveData.isFinished)
                    {
                        targetQuest.State = QuestState.IsFinished;
                    } else if (targetQuest.CurrentProgress >= targetQuest.TargetProgress)
                    {
                        targetQuest.State = QuestState.CanTakeReward;
                    }
                }
            };
            
            SaveLoadSystem.LoadQuest();

            GameManager.instance.OnStageSceneLoaded += () =>
            {
                GameManager.instance.Stage.UI.OnDirectionChangeButtonClicked += OnChangeDirection;
            };
            GameManager.instance.OnStageSceneUnloaded += () =>
            {
                SaveLoadSystem.SaveProgress();
            };
            
            FriendCollectionManager.instance.OnCollectFriend += OnCollectFinish;
            Item.OnInvoke += OnItemInvoke;
            Obstacle.OnInvoke += OnObstacleInvoke;
            Stage.OnNewLevelUnlocked += OnNewLevelUnlocked;
            Stage.OnStageClear += OnStageClear;
        }

        private void OnApplicationQuit()
        {
            SaveLoadSystem.SaveQuest();
        }

        private void OnChangeDirection()
        {
        }
        
        private void OnCollectFinish(FriendName arg1, int arg2)
        {
        }

        private void OnItemInvoke()
        {
        }

        private void OnObstacleInvoke()
        {
        }


        private void OnNewLevelUnlocked()
        {
        }
        
        private void OnStageClear(int stageLevel, int subLevel, int gainedStar)
        {
            if (stageLevel == 1 && subLevel == 0)
            {
                _questDict[1].ProgressAhead();
            }
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