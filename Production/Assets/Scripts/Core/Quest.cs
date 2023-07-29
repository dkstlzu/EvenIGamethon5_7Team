using System;
using System.Collections.Generic;
using dkstlzu.Utility;
using UnityEngine;

namespace MoonBunny
{
    public enum QuestType
    {
        None = -1,
        Default,
        Dependent,
        Repeatable,
    }

    public enum QuestName
    {
        None = -1,
        Default,

    }

    public enum QuestCheckTiming
    {
        OnStageClear,
        OnCollectionFinish,
        OnPlayApplication,
        OnQuitApplication,
        
    }
    
    [Serializable]
    public class Quest
    {
        public static Dictionary<QuestType, bool> Datas = new Dictionary<QuestType, bool>();

        [RuntimeInitializeOnLoadMethod]
        static void Init()
        {
            Datas.Clear();
        }

        public QuestType QuestType;
        public QuestCheckTiming CheckTiming;
        public int TargetProgress;
        public int CurrentProgress;
        public bool Enabled;

        public QuestName QuestName;
        public QuestName DependentTo;

        public event Action<QuestName> OnQuestCompleted;
        public event Action<int> OnQuestProgressChanged;

        protected virtual void ProgressAhead()
        {
            CurrentProgress++;
            CheckProgress();
        }

        protected virtual void ProgressAhead(int step)
        {
            CurrentProgress += step;
            CheckProgress();
        }

        public virtual void CheckProgress()
        {
            OnQuestProgressChanged?.Invoke(CurrentProgress);

            if (CurrentProgress >= TargetProgress) Complete();
        }

        public virtual void CheckProgress(FriendName friendName)
        {
            
        }

        public virtual void CheckProgress(int clearStage, int clearLevel, int gainedStar)
        {
            
        }

        protected virtual void Complete()
        {
            OnQuestCompleted?.Invoke(QuestName);
        }

        public void Enable()
        {
            Enabled = true;
        }

        public void Disable()
        {
            Enabled = false;
        }
    }

    [Serializable]
    public class DependentQuest : Quest
    {
        public Quest DependentOn;

        public DependentQuest(Quest dependentOn)
        {
            DependentOn = dependentOn;
            DependentOn.OnQuestCompleted += (type) => Enable();
        }
    }

    [Serializable]
    public class RepeatableQuest : Quest
    {
        public RepeatableQuest()
        {
            OnQuestCompleted += (type) => Repeat();
        }

        public void Repeat()
        {
            CurrentProgress = 0;
        }
    }
}