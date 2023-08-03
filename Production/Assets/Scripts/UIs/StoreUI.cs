﻿using System;
using UnityEngine;
using TMPro;
using DG.Tweening;
using MoonBunny.Dev;

namespace MoonBunny.UIs
{
    public class StoreUI : UI
    {
        public int MaxSellNumberPerFriend;
        public TextMeshProUGUI NoticeText;
        public float NoticeTweenDuration;
        public const string SellLimitExceedText = "구매 가능한 최대숫자를 넘었습니다";

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
                if (GameManager.ProgressSaveData.CollectionSellDict[firendNameEnum] >= MaxSellNumberPerFriend)
                {
                    NoticeText.DOText(SellLimitExceedText, NoticeTweenDuration);
                    return;
                }

                
            }

        }

        void BuyMemory(FriendName friendName)
        {
            
        }
    }
}