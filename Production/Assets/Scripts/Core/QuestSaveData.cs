using System;
using System.Collections.Generic;
using UnityEngine;

namespace MoonBunny
{
    [Serializable]
    public class LegacyQuestSaveData
    {
        [SerializeField] private List<QuestItemSaveData> _questClearList = new List<QuestItemSaveData>();

        public List<QuestItemSaveData> QuestClearList => _questClearList;

        public int JumpCount;
        public int ChangeDirectionCount;
        public int SideWallCollisionCount;
        public int ItemTakenCount;
        public int ObstacleCollisionCount;
        public int GoldGetCount;
        public int DiamondGetCount;

    }
    
    [Serializable]
    public class QuestSaveData
    {
        [SerializeField] private List<QuestItemSaveData> _questClearList = new List<QuestItemSaveData>();

        public List<QuestItemSaveData> QuestClearList => _questClearList;

        public int JumpCount;
        public int ChangeDirectionCount;
        public int SideWallCollisionCount;
        public int ItemTakenCount;
        public int ObstacleCollisionCount;
        public int GoldGetCount;
        public int DiamondGetCount;

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
        
        public static QuestSaveData MigrateFromLegacy(LegacyQuestSaveData data)
        {
            QuestSaveData newData = GetDefaultSaveData();

            foreach (QuestItemSaveData itemData in data.QuestClearList)
            {
                newData.QuestClearList.Add(itemData);
            }

            newData.JumpCount = data.JumpCount;
            newData.ChangeDirectionCount = data.ChangeDirectionCount;
            newData.SideWallCollisionCount = data.SideWallCollisionCount;
            newData.ItemTakenCount = data.ItemTakenCount;
            newData.ObstacleCollisionCount = data.ObstacleCollisionCount;
            newData.GoldGetCount = data.GoldGetCount;
            newData.DiamondGetCount = data.DiamondGetCount;

            return newData;
        }
    }
}