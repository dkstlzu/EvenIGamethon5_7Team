using System;
using System.Collections.Generic;
using dkstlzu.Utility;
using UnityEngine;

namespace MoonBunny
{
    [Serializable]
    public class QuestSaveData
    {
        public int Id;
        public int CurrentProgress;
        public bool isFinished;
        
        public QuestSaveData(int id)
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

        public bool Enabled = true;
        public bool Hidden = false;
        public bool isFinished;

        public QuestSaveData SaveData { get; set; }
        
        public int PercentProgress => (int)((float)CurrentProgress * 100 / TargetProgress);
        public bool CanTakeReward => CurrentProgress >= TargetProgress && !isFinished;

        public event Action OnQuestCompleted;

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
                OnQuestCompleted?.Invoke();
            }

            SaveData.CurrentProgress = CurrentProgress;
            
            GameManager.instance.SaveProgress();
        }

        public void TakeReward()
        {
            if (Repeatable)
            {
                CurrentProgress -= TargetProgress;
            }
            else
            {
                isFinished = true;
            }

            SaveData.isFinished = isFinished;
            SaveData.CurrentProgress = CurrentProgress;
            
            GameManager.instance.GoldNumber += Reward.GoldReward;
            GameManager.instance.DiamondNumber += Reward.DiamondReward;
            
            FriendCollectionManager.instance.Collect(Reward.MemoryTarget, Reward.MemoryNumber);
            
            GameManager.instance.SaveProgress();
        }
    }
}