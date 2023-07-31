using System;
using UnityEngine;
using TMPro;
using DG.Tweening;

namespace MoonBunny.UIs
{
    public class StoreUI : UI
    {
        public int MaxSellNumberPerFriend;
        public TextMeshProUGUI NoticeText;
        public float NoticeTweenDuration;
        public const string SellLimitExceedText = "구매 가능한 최대숫자를 넘었습니다";

        public void OnGatchaClicked()
        {
            
        }

        public void DoGatcha()
        {
            
        }

        public void OnMemoryClicked(string friendName)
        {
            FriendName firendNameEnum;

            if (Enum.TryParse<FriendName>(friendName, out firendNameEnum))
            {
                if (GameManager.SaveData.CollectionSellDict[firendNameEnum] >= MaxSellNumberPerFriend)
                {
                    NoticeText.DOText(SellLimitExceedText, NoticeTweenDuration);
                    return;
                }

                
            }

        }

        public void BuyMemory(FriendName friendName)
        {
            
        }
    }
}