﻿using System;
using System.Collections.Generic;
using dkstlzu.Utility;
using UnityEngine;

namespace MoonBunny
{
    [Serializable]
    public class QuestItemSaveData
    {
        public int Id;
        public int CurrentProgress;
        public bool isFinished;
        
        public QuestItemSaveData(int id)
        {
            Id = id;
        }
    }

    [Serializable]
    public struct QuestReward
    {
        public int GoldReward;
        public int DiamondReward;
        public FriendName MemoryTarget;
        public int MemoryNumber;
    }

    public enum QuestState
    {
        None = -1,
        Enabled,
        Disabled,
        Hidden,
        CanTakeReward,
        IsFinished,
    }
    
    [Serializable]
    public class Quest
    {
        public int Id;
        public int TargetProgress;
        public int CurrentProgress;
        
        public int DependentId = -1;
        public bool Repeatable;
        public string DescriptionText;
        public string DescriptionTextOnDisabled;
        public string DescriptionTextOnHidden;

        public QuestReward Reward;
        [SerializeField] private QuestState _state;

        public QuestState State
        {
            get => _state;
            set
            {
                _state = value;
                
                switch (_state)
                {
                    case QuestState.Enabled:
                        break;
                    case QuestState.Disabled:
                        break;
                    case QuestState.Hidden:
                        break;
                    case QuestState.CanTakeReward:
                        break;
                    case QuestState.IsFinished:
                        break;
                }
                
                OnStateChanged?.Invoke(_state);
            }
        }

        public event Action<QuestState> OnStateChanged;

        public QuestItemSaveData ItemSaveData { get; set; }
        
        public int PercentProgress => (int)((float)CurrentProgress * 100 / TargetProgress);

        public void ProgressAhead()
        {
            CurrentProgress++;
            CheckProgress();
        }

        public void ProgressAhead(int step)
        {
            CurrentProgress += step;
            CheckProgress();
        }

        public virtual void CheckProgress()
        {
            if (CurrentProgress >= TargetProgress)
            {
                State = QuestState.CanTakeReward;
            }

            ItemSaveData.CurrentProgress = CurrentProgress;
        }

        public void TakeReward()
        {
            if (Repeatable)
            {
                CurrentProgress -= TargetProgress;
                State = QuestState.Enabled;
            }
            else
            {
                State = QuestState.IsFinished;
            }

            ItemSaveData.isFinished = State == QuestState.IsFinished;
            ItemSaveData.CurrentProgress = CurrentProgress;
            
            GameManager.instance.GoldNumber += Reward.GoldReward;
            GameManager.instance.DiamondNumber += Reward.DiamondReward;

            if (Reward.MemoryTarget != FriendName.None && Reward.MemoryNumber > 0)
            {
                FriendCollectionManager.instance.Collect(Reward.MemoryTarget, Reward.MemoryNumber);
            }
            
            GameManager.instance.SaveProgress();
            QuestManager.instance.SaveLoadSystem.SaveQuest();
        }
    }
}