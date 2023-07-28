using System;
using dkstlzu.Utility;

namespace MoonBunny
{
    public enum QuestType
    {
        None = -1,
        Default,
        Sample,
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
        public QuestCheckTiming CheckTiming;
        public EnumDictElement<QuestType, bool> Data;

        public virtual void CheckProgress()
        {
            
        }

        public virtual void CheckProgress(FriendName friendName)
        {
            
        }

        public virtual void CheckProgress(int clearStage, int clearLevel, int gainedStar)
        {
            
        }
    }
}