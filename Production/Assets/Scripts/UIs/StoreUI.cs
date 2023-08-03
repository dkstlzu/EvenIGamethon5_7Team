using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using MoonBunny.Dev;

namespace MoonBunny.UIs
{
    public class StoreUI : UI
    {
        public List<TextMeshProUGUI> MemoryPriceTextList;
        public List<int> MemoryPriceList;

        public ConfirmUI ConfirmUI;
        public string ConfirmDescription = @"정말 구매하시겠습니까?";
        
        public TextMeshProUGUI NoticeText;
        public float NoticeTweenDuration;
        public const string SellLimitExceedText = "구매 가능한 최대숫자를 넘었습니다";

        private void Awake()
        {
            for (int i = 0; i < MemoryPriceTextList.Count; i++)
            {
                MemoryPriceTextList[i].text = MemoryPriceList[i].ToString();
            }
            
            OnOpen += Rebuild;
        }

        private void Rebuild()
        {
            
        }

        public void OnNormalGotchaClicked()
        {
            MoonBunnyLog.print("Normal Gotcha");
        }
        
        public void OnSpecialGotchaClicked()
        {
            MoonBunnyLog.print("Special Gotcha");
        }


        private void DoGotcha()
        {
            
        }

        public void OnMemoryPurchase(string friendName)
        {
            MoonBunnyLog.print($"Memory Purchase {friendName}");

            FriendName firendNameEnum;

            if (Enum.TryParse<FriendName>(friendName, out firendNameEnum))
            {
                if (GameManager.ProgressSaveData.CollectionSellDict[firendNameEnum] >= PreloadedResources.instance.FriendSpecList[(int)firendNameEnum].MaxPurchasableNumber)
                {
                    NoticeText.DOText(SellLimitExceedText, NoticeTweenDuration);
                    return;
                }
                
                ConfirmUI.Description.text = ConfirmDescription;
                ConfirmUI.OnConfirm.AddListener(() => BuyMemory(firendNameEnum));
                ConfirmUI.Open();
            }
        }

        void BuyMemory(FriendName friendName)
        {
            GameManager.ProgressSaveData.CollectionSellDict[friendName]++;
            FriendCollectionManager.instance.Collection.Datas[(int)friendName].CurrentCollectingNumber++;
            GameManager.instance.SaveProgress();
        }
    }
}