using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MoonBunny.UIs
{
    public class QuestUIItem : MonoBehaviour
    {
        public TextMeshProUGUI Description;
        public Toggle Checker;
        public QuestName QuestName;

        private Quest _quest;

        void Awake()
        {
            _quest = QuestManager.instance.GetQuest(QuestName);
        }
    }
}