﻿using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using MoonBunny.Dev;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace MoonBunny.UIs
{
    [Serializable]
    public struct GotchaReward
    {
        public int GoldNumber;
        public int DiamondNumber;
        public FriendName MemoryType;
        public int MemoryNumber;
        public float Potential;

        public override string ToString()
        {
            return $"Gold {GoldNumber}, Diamond {DiamondNumber}, Memory of {MemoryType} {MemoryNumber} with {Potential} potential";
        }
    }
    
    public class StoreUI : UI
    {
        public List<TextMeshProUGUI> MemoryPurchaseTextList;
        public List<TextMeshProUGUI> MemoryPriceTextList;
        public List<int> MemoryPriceList;

        public Image NormalGotchaImage;
        public List<GotchaReward> NormalGotchaRewards;
        public Image SpecialGotchaImage;
        public List<GotchaReward> SpecialGotchaRewards;

        public ConfirmUI ConfirmUI;
        public string ConfirmDescription = @"정말 구매하시겠습니까?";
        public string NotEnoughMoneyDescription = @"돈이 모자라네요ㅠㅠ";
        
        public TextMeshProUGUI NoticeText;
        public float NoticeTweenDuration;
        public const string AlreadyCollectedText = "이미 수집 완료한 친구입니다.";
        public const string SellLimitExceedText = "구매 가능한 최대숫자를 넘었습니다";

        public const int NORMAL_GOTCHA_GOLD_COST = 150;
        public const int SPECIAL_GOTCHA_DIAMOND_COST = 3;
        
        private void Start()
        {
            for (int i = 0; i < MemoryPurchaseTextList.Count; i++)
            {
                MemoryPurchaseTextList[i].text = $"{GameManager.ProgressSaveData.CollectionSellDict[(FriendName)(i+1)]}/{PreloadedResources.instance.FriendSpecList[i+1].MaxPurchasableNumber}";
            }
            
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
            if (GameManager.instance.GoldNumber < NORMAL_GOTCHA_GOLD_COST)
            {
                NotEnoughMoney();
                return;
            }

            ConfirmUI.Description.text = ConfirmDescription;
            ConfirmUI.OnConfirm.AddListener(DoNormalGotcha);
            ConfirmUI.Open();
        }
        
        public void OnSpecialGotchaClicked()
        {
            if (GameManager.instance.DiamondNumber < SPECIAL_GOTCHA_DIAMOND_COST)
            {
                NotEnoughMoney();
                return;
            }

            ConfirmUI.Description.text = ConfirmDescription;
            ConfirmUI.OnConfirm.AddListener(DoSpecialGotcha);
            ConfirmUI.Open();
        }


        private void DoNormalGotcha()
        {
            GameManager.instance.GoldNumber -= NORMAL_GOTCHA_GOLD_COST;

            GotchaReward reward = GetRandom(NormalGotchaRewards);
            
            if (EqualityComparer<GotchaReward>.Default.Equals(reward, default))
            {
                Debug.LogError("Random Error on gotcha. Check again");
                return;
            }
            
            GameManager.instance.GoldNumber += reward.GoldNumber;
            GameManager.instance.DiamondNumber += reward.DiamondNumber;

            if (reward.MemoryNumber > 0 && reward.MemoryType != FriendName.None && !FriendCollectionManager.instance.CollectFinished(reward.MemoryType))
            {
                FriendCollectionManager.instance.Collect(reward.MemoryType, reward.MemoryNumber);
            }
            
            MoonBunnyLog.print($"NormalGotcha reward {reward}");
            GameManager.instance.SaveProgress();
        }
        
        private void DoSpecialGotcha()
        {
            GameManager.instance.DiamondNumber -= SPECIAL_GOTCHA_DIAMOND_COST;

            GotchaReward reward = GetRandom(SpecialGotchaRewards);
            
            if (EqualityComparer<GotchaReward>.Default.Equals(reward, default))
            {
                Debug.LogError("Random Error on gotcha. Check again");
                return;
            }
            
            GameManager.instance.GoldNumber += reward.GoldNumber;
            GameManager.instance.DiamondNumber += reward.DiamondNumber;

            if (reward.MemoryNumber > 0 && !FriendCollectionManager.instance.CollectFinished(reward.MemoryType))
            {
                FriendCollectionManager.instance.Collect(reward.MemoryType, reward.MemoryNumber);
            }
            
            MoonBunnyLog.print($"SpecialGotcha reward {reward}");
            GameManager.instance.SaveProgress(); 
        }

        GotchaReward GetRandom(IEnumerable<GotchaReward> rewards)
        {
            float total = 0;
            List<float> potentialCumulativeSum = new List<float>();

            using (IEnumerator<GotchaReward> enumerator = rewards.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    total += enumerator.Current.Potential;
                    potentialCumulativeSum.Add(total);
                }
            }

            float randomValue = Random.Range(0, total);
            
            using (IEnumerator<GotchaReward> enumerator = rewards.GetEnumerator())
            {
                int index = 0;
                while (enumerator.MoveNext())
                {
                    if (randomValue <= potentialCumulativeSum[index])
                    {
                        return enumerator.Current;
                    }

                    index++;
                }
            }

            return default;
        }

        public void OnMemoryPurchase(string friendName)
        {
            FriendName friendNameEnum;

            if (Enum.TryParse<FriendName>(friendName, out friendNameEnum))
            {
                if (GameManager.instance.DiamondNumber < MemoryPriceList[((int)friendNameEnum) - 1])
                {
                    NotEnoughMoney();    
                    return;
                }
                
                if (FriendCollectionManager.instance.CollectFinished(friendNameEnum))
                {
                    NoticeText.DOText(AlreadyCollectedText, NoticeTweenDuration);
                    return;
                }
                if (GameManager.ProgressSaveData.CollectionSellDict[friendNameEnum] >= PreloadedResources.instance.FriendSpecList[(int)friendNameEnum].MaxPurchasableNumber)
                {
                    NoticeText.DOText(SellLimitExceedText, NoticeTweenDuration);
                    return;
                }

                GameManager.instance.DiamondNumber -= MemoryPriceList[((int)friendNameEnum) - 1];
                ConfirmUI.Description.text = ConfirmDescription;
                ConfirmUI.OnConfirm.AddListener(() => BuyMemory(friendNameEnum));
                ConfirmUI.Open();
            }
        }

        void BuyMemory(FriendName friendName)
        {
            MoonBunnyLog.print($"Memory Purchase {friendName}");

            GameManager.ProgressSaveData.CollectionSellDict[friendName]++;
            FriendCollectionManager.instance.Collection.Datas[(int)friendName].CurrentCollectingNumber++;
            GameManager.instance.SaveProgress();
        }

        void NotEnoughMoney()
        {
            ConfirmUI.Description.text = NotEnoughMoneyDescription;
            ConfirmUI.Open();
        }
    }
}