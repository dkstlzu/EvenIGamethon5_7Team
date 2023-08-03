using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MoonBunny.UIs
{
    public class QuestUIItem : MonoBehaviour
    {
        public int TargetQuestId;
        public Button Button;
        public TextMeshProUGUI Description;
        public TextMeshProUGUI Progress;

        private Quest _quest;
        

        public void Set(Quest quest)
        {
            TargetQuestId = quest.Id;

            _quest = quest;
            _quest.OnStateChanged += (state) => Rewind();
            
            Rewind();
        }

        public void Rewind()
        {
            Progress.text = $"{_quest.PercentProgress}%";

            switch (_quest.State)
            {
                case QuestState.Enabled:
                    Button.interactable = false;
                    Description.text = _quest.DescriptionText;
                    Description.color = Color.black;
                    break;
                case QuestState.Disabled:
                    Button.interactable = false;
                    Description.text = _quest.DescriptionTextOnDisabled;
                    Description.color = MoonBunnyColor.DisabledColor;
                    break;
                case QuestState.Hidden:
                    Button.interactable = false;
                    Description.text = _quest.DescriptionTextOnHidden;
                    Description.color = MoonBunnyColor.QuestHiddenColor;
                    break;
                case QuestState.CanTakeReward:
                    Button.interactable = true;
                    Description.text = _quest.DescriptionText;
                    Description.color = MoonBunnyColor.QuestCompleteColor;
                
                    transform.SetAsFirstSibling();
                    break;
                case QuestState.IsFinished:
                    Button.interactable = false;
                    Description.text = "<s>" + _quest.DescriptionText + "</s>";
                    Description.color = MoonBunnyColor.DisabledColor;
                
                    transform.SetAsLastSibling();
                    break;
            }
        }

        public void OnTakeReward()
        {
            _quest.TakeReward();
        }
    }
}