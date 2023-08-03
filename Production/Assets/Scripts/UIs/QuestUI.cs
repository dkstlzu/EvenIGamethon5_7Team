using System;
using System.Collections.Generic;
using System.IO;
using dkstlzu.Utility;
using UnityEngine;
using UnityEngine.Serialization;

namespace MoonBunny.UIs
{
    public class QuestUI : UI
    {
        public GameObject QuestUIItemPrefab;
        public Transform UIItemParent;
        public ReadOnlyDict<int, QuestUIItem> QuestUIItemDict = new ReadOnlyDict<int, QuestUIItem>();

        private void Start()
        {
            Quest[] quests = QuestManager.instance.GetAllQuest();
            List<QuestUIItem> UIItemList = new List<QuestUIItem>();

            for (int i = 0; i < quests.Length; i++)
            {
                UIItemList.Add(Instantiate(QuestUIItemPrefab, UIItemParent).GetComponent<QuestUIItem>().GetComponent<QuestUIItem>());
            }

            for (int i = 0; i < quests.Length; i++)
            {
                UIItemList[i].Set(quests[i]);
                QuestUIItemDict.Add(UIItemList[i].TargetQuestId, UIItemList[i]);
            }

            OnOpen += () =>
            {
                foreach (var pair in QuestUIItemDict)
                {
                    pair.Value.Rewind();
                }
            };
        }
    }
}