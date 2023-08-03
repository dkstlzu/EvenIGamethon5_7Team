using System;
using System.Collections.Generic;
using UnityEngine;

namespace MoonBunny
{
    [Serializable]
    public class QuestSaveData
    {
        [SerializeField] private List<QuestItemSaveData> _questClearList = new List<QuestItemSaveData>();

        public List<QuestItemSaveData> QuestClearList => _questClearList;

        public static QuestSaveData GetDefaultSaveData()
        {
            QuestSaveData saveData = new QuestSaveData();
            
            QuestSpec[] questSpecs = Resources.LoadAll<QuestSpec>("Specs/Quest/");
            
            for (int i = 0; i < questSpecs.Length; i++)
            {
                saveData.QuestClearList.Add(new QuestItemSaveData(questSpecs[i].Id));
            }

            return saveData;
        }

        public static QuestSaveData GetFullSaveData()
        {
            QuestSaveData saveData = GetDefaultSaveData();
            
            QuestSpec[] questSpecs = Resources.LoadAll<QuestSpec>("Specs/Quest/");
            
            for (int i = 0; i < questSpecs.Length; i++)
            {
                saveData.QuestClearList[i].CurrentProgress = questSpecs[i].TargetProgress;
            }

            return saveData;
        }
    }
}