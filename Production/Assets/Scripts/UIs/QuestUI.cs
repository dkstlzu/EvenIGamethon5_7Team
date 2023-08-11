using System;
using System.Collections.Generic;
using System.IO;
using dkstlzu.Utility;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace MoonBunny.UIs
{
    public class QuestUI : UI
    {
        public static int S_FinishedNumber;
        public static int S_CanTakeRewardNumber;
        public static int S_EnabledNumber;
        public static int S_DisabledNumber;
        public static int S_HiddenNumber;
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void InitNumber()
        {
            S_FinishedNumber = 0;
            S_CanTakeRewardNumber = 0;
            S_EnabledNumber = 0;
            S_DisabledNumber = 0;
            S_HiddenNumber = 0;
        }
        
        public GameObject QuestUIItemPrefab;
        public Transform UIItemParent;
        public ReadOnlyDict<int, QuestUIItem> QuestUIItemDict = new ReadOnlyDict<int, QuestUIItem>();
        [SerializeField] private VerticalLayoutGroup _layoutGroup;

        private void Start()
        {
            InitNumber();
            
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

            GameManager.instance.StartSceneUI.FriendSelectUI.QuestRewardNoticeImage.enabled = S_CanTakeRewardNumber > 0;
            
            Rebuild();
            
            OnExit += () =>
            {
                GameManager.instance.StartSceneUI.FriendSelectUI.Open();
            };
        }

        protected override void Rebuild()
        {
            _layoutGroup.enabled = false;
                
            foreach (var pair in QuestUIItemDict)
            {
                pair.Value.Rewind();
            }
                
            _layoutGroup.enabled = true;
        }
    }
}