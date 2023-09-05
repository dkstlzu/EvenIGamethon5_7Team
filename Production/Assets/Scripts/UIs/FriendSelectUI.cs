using System;
using System.Collections.Generic;
using dkstlzu.Utility;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MoonBunny.UIs
{
    public class FriendSelectUI : UI
    {
        [Serializable]
        public class FriendLibraryUI
        {
            public Button SelectButton;
            public TextMeshProUGUI NameText;
            public Sprite ProfileSprite;
        }
        
        public List<FriendLibraryUI> FriendLibraryUIList;
        public FriendProfileUI FriendProfileUI;
        public AnimationCurve FriendLibraryImageCurve;
        
        public Image QuestRewardNoticeImage;

        public TextMeshProUGUI ProgressText;
        public Slider ProgressBar;
        public GameObject ShowEnding;

        protected override void Awake()
        {
            base.Awake();
            Rebuild();
        }

        protected override void Rebuild()
        {
            QuestRewardNoticeImage.enabled = QuestUI.S_CanTakeRewardNumber > 0;
            
            for (int i = 0; i < FriendLibraryUIList.Count; i++)
            {
                FriendCollection.Data data = FriendCollectionManager.instance[(FriendName)i];
                
                if (data.IsFinish())
                {
                    FriendLibraryUIList[i].SelectButton.interactable = true;
                    string name = StringValue.GetStringValue(data.Name);
                    FriendLibraryUIList[i].NameText.text = name;
                    FriendLibraryUIList[i].SelectButton.image.color = Color.white;
                }
                else
                {
                    float value = FriendLibraryImageCurve.Evaluate(data.GetPercent());
                    FriendLibraryUIList[i].SelectButton.image.color = new Color(value, value, value, 1);
                }
            }

            RebuildProgressBar();
        }

        public void RebuildProgressBar()
        {
            float progressValue = (float)FriendCollectionManager.instance.CurrentCollectionNumber /
                                  FriendCollectionManager.instance.TotalCollectionNumber;
            ProgressText.text = $"달성률 {(int)(progressValue * 100)}%";
            ProgressBar.value = progressValue;
            
            if (progressValue >= 1)
            {
                ShowEnding.SetActive(true);
            }
            else
            {
                ShowEnding.SetActive(false);
            }
        }
        
        public void OnFriendButtonClicked(string friendName)
        {
            FriendName name;

            if (Enum.TryParse(friendName, out name))
            {
                FriendProfileUI.Set(name);
                FriendProfileUI.Open();
            }
        }
    }
}