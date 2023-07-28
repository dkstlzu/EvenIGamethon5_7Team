using System;
using System.Collections.Generic;
using dkstlzu.Utility;
using UnityEngine;

namespace MoonBunny
{
    public class QuestManager : Singleton<QuestManager>
    {
        public List<Quest> QuestList;

        private Action _onApplicationQuit;
        private void Awake()
        {
            foreach (Quest quest in QuestList)
            {
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
    }
}