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

        public TextMeshProUGUI ProgressText;
        public Slider ProgressBar;
        public GameObject ShowEnding;

        protected override void Rebuild()
        {
            for (int i = 0; i < FriendLibraryUIList.Count; i++)
            {
                if (!FriendCollectionManager.instance.Collection.Datas[i].IsFinish()) continue;
                
                FriendLibraryUIList[i].SelectButton.interactable = true;
                FriendLibraryUIList[i].SelectButton.image.sprite = FriendProfileUI.FriendSpriteList[i];
                string name = StringValue.GetStringValue(FriendCollectionManager.instance.Collection.Datas[i].Name);
                FriendLibraryUIList[i].NameText.text = name;
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