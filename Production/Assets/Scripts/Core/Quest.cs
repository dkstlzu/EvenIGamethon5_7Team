﻿using System;
using System.Collections.Generic;
using dkstlzu.Utility;
using MoonBunny.Dev;
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
        public static event Action<int> OnQusetCanTakeReward;
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void EventInit()
        {
            OnQusetCanTakeReward = null;
        }
        
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
                switch (value)
                {
                    case QuestState.Enabled:
                        break;
                    case QuestState.Disabled:
                        break;
                    case QuestState.Hidden:
                        break;
                    case QuestState.CanTakeReward:
                        OnQusetCanTakeReward?.Invoke(Id);
                        break;
                    case QuestState.IsFinished:
                        break;
                }
                
                _state = value;
                
                OnStateChanged?.Invoke(_state);
            }
        }

        public event Action<QuestState> OnStateChanged;

        public QuestItemSaveData ItemSaveData { get; set; }
        
        public int PercentProgress => (int)((float)CurrentProgress * 100 / TargetProgress);

        public void ProgressAhead()
        {
            if (State == QuestState.IsFinished) return;
            
            CurrentProgress++;
            CheckProgress();
        }

        public void ProgressAhead(int step)
        {
            if (State == QuestState.IsFinished) return;

            CurrentProgress += step;
            CheckProgress();
        }

        public virtual void CheckProgress()
        {
            if (CurrentProgress >= TargetProgress)
            {
                State = QuestState.CanTakeReward;

                if (!Repeatable) CurrentProgress = TargetProgress;
            }

            if (ItemSaveData != null)
            {
                ItemSaveData.CurrentProgress = CurrentProgress;
            }
            else
            {
                MoonBunnyLog.print($"Quest {Id} has null ref of ItemSaveData");
            }
        }

        public void TakeReward()
        {
            if (Repeatable)
            {
                CurrentProgress -= TargetProgress;
                if (CurrentProgress >= TargetProgress)
                {
                    State = QuestState.CanTakeReward;
                }
                else
                {
                    State = QuestState.Enabled;
                }
            }
            else
            {
                State = QuestState.IsFinished;
            }

            ItemSaveData.isFinished = State == QuestState.IsFinished;
            ItemSaveData.CurrentProgress = CurrentProgress;
            
            QuestManager.instance.GetGold(Reward.GoldReward, false);
            QuestManager.instance.GetDiamond(Reward.DiamondReward, false);

            if (Reward.MemoryTarget != FriendName.None && Reward.MemoryNumber > 0)
            {
                FriendCollectionManager.instance.Collect(Reward.MemoryTarget, Reward.MemoryNumber);
            }

            if (Reward.GoldReward > 0 || Reward.DiamondReward > 0 || (Reward.MemoryTarget != FriendName.None && Reward.MemoryNumber > 0))
            {
                GameManager.instance.SaveProgress();
            }
            QuestManager.instance.SaveLoadSystem.SaveQuest();
        }
    }
}