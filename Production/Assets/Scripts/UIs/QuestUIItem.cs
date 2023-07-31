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
            
            Rewind();
        }

        public void Rewind()
        {
            Progress.text = $"{_quest.PercentProgress}%";

            if (_quest.isFinished)
            {
                Button.interactable = false;
                Description.text = "<s>" + _quest.DescriptionText + "</s>";
                Description.color = MoonBunnyColor.DisabledColor;
            }
            else if (_quest.Hidden)
            {
                Button.interactable = false;
                Description.text = _quest.DescriptionTextOnHidden;
                Description.color = MoonBunnyColor.QuestHiddenColor;
            }
            else if (!_quest.Enabled)
            {
                Button.interactable = false;
                Description.text = _quest.DescriptionTextOnDisabled;
                Description.color = MoonBunnyColor.DisabledColor;
            }
            else if (_quest.CanTakeReward)
            {
                Button.interactable = true;
                Description.text = _quest.DescriptionText;
                Description.color = MoonBunnyColor.QuestCompleteColor;
            }
            else
            {
                Button.interactable = false;
                Description.text = _quest.DescriptionText;
                Description.color = Color.black;
            }
        }

        public void OnTakeReward()
        {
            _quest.TakeReward();
            Rewind();
        }
    }
}