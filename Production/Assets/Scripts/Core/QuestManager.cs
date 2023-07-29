using System;
using System.Collections.Generic;
using dkstlzu.Utility;
using UnityEngine;

namespace MoonBunny
{
    [Serializable]
    public class TestList : InheritSwapList<Quest> {}

    [Serializable]
    public class TestList2 : List<Quest> {}

    public class QuestManager : Singleton<QuestManager>
    {
        public InheritanceList<Quest> InheritanceQuestList;
        public List<Quest> QuestListNormal;
        public TestList TestList;
        public TestList2 TestList2;
        public InheritSwapList<Quest> QuestList;

        private Action _onApplicationQuit;

        private void Reset()
        {
            InheritanceQuestList.Add(new Quest());
            InheritanceQuestList.Add(new Quest());
            InheritanceQuestList.Add(new Quest());
            InheritanceQuestList.Add(new Quest());
            InheritanceQuestList.Add(new Quest());
        }

        private void Awake()
        {
            for (int i = 0; i < QuestList.Count; i++)
            {
                Quest quest = QuestList[i];

                switch (quest.QuestType)
                {
                    case QuestType.Default:
                    QuestList.InheritanceList.Add(new Quest());
                    break;
                    case QuestType.Dependent:
                    QuestList.InheritanceList.Add(new DependentQuest(QuestManager.instance.GetQuest(quest.DependentTo)));
                    break;
                    case QuestType.Repeatable:
                    QuestList.InheritanceList.Add(new ReapeatableQuest());
                    break;
                }

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