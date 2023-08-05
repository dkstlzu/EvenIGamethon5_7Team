using System;
using dkstlzu.Utility;
using MoonBunny.Dev;
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
        public TextMeshProUGUI RewardText;

        private Quest _quest;
        private QuestState _lastState = QuestState.None;

        public void Set(Quest quest)
        {
            TargetQuestId = quest.Id;

            _quest = quest;
            _quest.OnStateChanged += Rewind;
        }

        private void OnDestroy()
        {
            _quest.OnStateChanged -= Rewind;
        }

        private void Rewind(QuestState state)
        {
            Rewind();
        }

        public void Rewind()
        {
            Progress.text = $"{_quest.PercentProgress}%";

            if (_lastState == _quest.State) return;

            switch (_lastState)
            {
                case QuestState.Enabled:
                    QuestUI.S_EnabledNumber--;
                    break;
                case QuestState.Disabled:
                    QuestUI.S_DisabledNumber--;
                    break;
                case QuestState.Hidden:
                    QuestUI.S_HiddenNumber--;
                    break;
                case QuestState.CanTakeReward:
                    QuestUI.S_CanTakeRewardNumber--;
                    break;
                case QuestState.IsFinished:
                    QuestUI.S_FinishedNumber--;
                    break;
            }

            RewardText.text = "보상 :";
            if (_quest.Reward.GoldReward > 0)
            {
                RewardText.text += $" 골드 {_quest.Reward.GoldReward}개";
            }

            if (_quest.Reward.DiamondReward > 0)
            {
                RewardText.text += $" 다이아 {_quest.Reward.DiamondReward}개";
            }

            if (_quest.Reward.MemoryNumber > 0)
            {
                RewardText.text += $" {StringValue.GetStringValue(_quest.Reward.MemoryTarget)}조각 {_quest.Reward.MemoryNumber}개";
            }
            
            switch (_quest.State)
            {
                case QuestState.Enabled:
                    Button.interactable = false;
                    Description.text = _quest.DescriptionText;
                    Description.color = Color.black;
                    
                    QuestUI.S_EnabledNumber++;
                    transform.SetSiblingIndex(QuestUI.S_CanTakeRewardNumber);
                    break;
                case QuestState.Disabled:
                    Button.interactable = false;
                    Description.text = _quest.DescriptionTextOnDisabled;
                    Description.color = MoonBunnyColor.DisabledColor;
                    RewardText.text = $"보상 : ????";

                    QuestUI.S_DisabledNumber++;
                    transform.SetSiblingIndex(QuestUI.S_CanTakeRewardNumber + QuestUI.S_EnabledNumber + QuestUI.S_HiddenNumber);
                    break;
                case QuestState.Hidden:
                    Button.interactable = false;
                    Description.text = _quest.DescriptionTextOnHidden;
                    Description.color = MoonBunnyColor.QuestHiddenColor;
                    RewardText.text = $"보상 : ????";

                    QuestUI.S_HiddenNumber++;
                    transform.SetSiblingIndex(QuestUI.S_CanTakeRewardNumber + QuestUI.S_EnabledNumber);
                    break;
                case QuestState.CanTakeReward:
                    Button.interactable = true;
                    Description.text = _quest.DescriptionText;
                    Description.color = MoonBunnyColor.QuestCompleteColor;

                    QuestUI.S_CanTakeRewardNumber++;
                    transform.SetAsFirstSibling();
                    break;
                case QuestState.IsFinished:
                    Button.interactable = false;
                    Description.text = "<s>" + _quest.DescriptionText + "</s>";
                    Description.color = MoonBunnyColor.DisabledColor;

                    QuestUI.S_FinishedNumber++;
                    transform.SetAsLastSibling();
                    break;
            }

            _lastState = _quest.State;
        }

        public void OnTakeReward()
        {
            _quest.TakeReward();
        }
    }
}