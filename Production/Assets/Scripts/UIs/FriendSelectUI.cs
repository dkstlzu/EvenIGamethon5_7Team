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

        private void Start()
        {
            for (int i = 0; i < FriendLibraryUIList.Count; i++)
            {
                if (!FriendCollectionManager.instance.Collection.Datas[i].IsFinish()) continue;
                
                FriendLibraryUIList[i].SelectButton.interactable = true;
                string name = StringValue.GetStringValue(FriendCollectionManager.instance.Collection.Datas[i].Name);
                FriendLibraryUIList[i].NameText.text = name;
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