using System;
using System.Collections.Generic;
using System.IO;
using dkstlzu.Utility;
using UnityEngine;

namespace MoonBunny.UIs
{
    public class QuestUI : UI
    {
        public GameObject QuestUIItemPrefab;
        public Transform UIItemParent;
        public List<QuestUIItem> QuestUIItemList;

        private void Start()
        {
            foreach (Quest quest in QuestManager.instance.GetAllQuest())
            {
                QuestUIItem item = Instantiate(QuestUIItemPrefab, UIItemParent).GetComponent<QuestUIItem>();
                item.Set(quest);
                QuestUIItemList.Add(item);
            }

            OnOpen += () =>
            {
                foreach (QuestUIItem item in QuestUIItemList)
                {
                    item.Rewind();
                }
            };
        }
    }
}