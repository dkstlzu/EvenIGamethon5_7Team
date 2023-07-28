using System;
using System.Collections.Generic;
using System.IO;
using dkstlzu.Utility;
using UnityEngine;

namespace MoonBunny.UIs
{
    public class QuestUI : UI
    {
        private string _questSpecPath = Path.Combine("Specs", "Quest");

        private string GetFullPath(string specName)
        {
            return Path.Combine(_questSpecPath, specName + ".asset");
        }

        [SerializeField] private List<QuestSpec> _questSpecList = new List<QuestSpec>();
        public List<QuestUIItem> QuestUIItemList;

        protected override void Reset()
        {
            base.Reset();
           
            QuestUIItemList.AddRange(GetComponentsInChildren<QuestUIItem>());
        }

        private void Awake()
        {
            QuestSpec[] specs = Resources.LoadAll<QuestSpec>(_questSpecPath);

            _questSpecList.AddRange(specs);

            for (int i = 0; i < Mathf.Min(QuestUIItemList.Count, _questSpecList.Count); i++)
            {
                QuestUIItemList[i].Description.text = _questSpecList[i].DescriptionText;
                QuestUIItemList[i].Checker.isOn = GameManager.instance.QuestClearDict[(QuestType)i];
            }
        }
    }
}