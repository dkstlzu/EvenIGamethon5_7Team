using System;
using System.Collections.Generic;
using dkstlzu.Utility;
using UnityEngine;

namespace MoonBunny
{
    public class QuestManager : Singleton<QuestManager>
    {
        private List<Quest> QuestList = new List<Quest>();
        public List<Quest> DefaultQuestList;
        public List<DependentQuest> DependentQuestList;
        public List<RepeatableQuest> RepeatableQuestList;

        private Action _onApplicationQuit;

        private void Awake()
        {
            QuestList.AddRange(DefaultQuestList);
            QuestList.AddRange(DependentQuestList);
            QuestList.AddRange(RepeatableQuestList);
            
            for (int i = 0; i < QuestList.Count; i++)
            {
                Quest quest = QuestList[i];
                
                switch (quest.CheckTiming)
                {
                    case QuestCheckTiming.OnStageClear:
                        Stage.OnStageClear += quest.CheckProgress;
                        break;
                    case QuestCheckTiming.OnCollectionFinish:
                        FriendCollectionManager.instance.OnCollectFriendFinish += quest.CheckProgress;
                        break;
                    case QuestCheckTiming.OnPlayApplication:
                        quest.CheckProgress();
                        break;
                    case QuestCheckTiming.OnQuitApplication:
                        _onApplicationQuit += quest.CheckProgress;
                        break;
                }
            }
        }

        private void OnApplicationQuit()
        {
            _onApplicationQuit?.Invoke();
        }

        public Quest GetQuest(QuestName name)
        {
            return QuestList.Find(e => e.QuestName == name);
        }
    }
}