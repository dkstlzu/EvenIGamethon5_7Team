using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using dkstlzu.Utility;
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
        public static event Action<GotchaReward> OnGotchaRewardGet;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void EventInit()
        {
            OnGotchaRewardGet = null;
        }
        
        public RectTransform Board;
        public RectTransform Contents;
        public List<TextMeshProUGUI> MemoryPurchaseTextList;
        public List<TextMeshProUGUI> MemoryPriceTextList;
        public List<int> MemoryPriceList;

        public float GotchaAnimationInterval;

        public List<string> NormalGotchaText;
        public Image NormalGotchaImage;
        public List<Sprite> NormalGotchaAnimationSprites;
        public GotchaData NormalGotchaData;
        public TextMeshProUGUI NormalGotchaResult;

        public List<string> SpecialGotchaText;
        public Image SpecialGotchaImage;
        public GotchaData SpecialGotchaData;
        public List<Sprite> SpecialGotchaAnimationSprites;
        public TextMeshProUGUI SpecialGotchaResult;

        private const float GOTCHA_RESULT_TWEEN_DURATION = 0.2f;

        public ConfirmUI ConfirmUI;
        public string ConfirmDescription = @"정말 구매하시겠습니까?";
        public string NotEnoughMoneyDescription = @"돈이 모자라네요ㅠㅠ";
        
        public TextMeshProUGUI NoticeText;
        public float NoticeTweenDuration;
        public const string AlreadyCollectedText = "이미 수집 완료한 친구입니다.";
        public const string SellLimitExceedText = "구매 가능한 최대숫자를 넘었습니다";
        public const string PurchaseSuccess = "구매 완료";

        public const int NORMAL_GOTCHA_GOLD_COST = 150;
        public const int SPECIAL_GOTCHA_DIAMOND_COST = 3;

        private const float TWEEN_DURATION = 0.5f;
        private const float NORMAL_GOTCHA_Y = 0f;
        private const float SPECIAL_GOTCHA_Y = 615f;
        private const float MEMORY_Y = 1270;

        private void Start()
        {
            OnExit += GameManager.instance.StartSceneUI.FriendSelectUI.Open;
        }

        protected override void Rebuild()
        {
            for (int i = 0; i < MemoryPurchaseTextList.Count; i++)
            {
                MemoryPurchaseTextList[i].text = $"{GameManager.ProgressSaveData.CollectionSellDict[(FriendName)(i+1)]}/{PreloadedResources.instance.FriendSpecList[i+1].MaxPurchasableNumber}";
            }
            
            for (int i = 0; i < MemoryPriceTextList.Count; i++)
            {
                MemoryPriceTextList[i].text = MemoryPriceList[i].ToString();
            }
        }

        public void OnNormalGotchaClicked()
        {
            if (GameManager.instance.GoldNumber < NORMAL_GOTCHA_GOLD_COST)
            {
                NotEnoughMoney();
                return;
            }

            ConfirmUI.Description.text = ConfirmDescription;
            ConfirmUI.AddConfirmListener(DoNormalGotcha);
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
            ConfirmUI.AddConfirmListener(DoSpecialGotcha);
            ConfirmUI.Open();
        }


        private void DoNormalGotcha()
        {
            _graphicRaycaster.enabled = false;

            Contents.DOLocalMoveY(Board.rect.yMax + NORMAL_GOTCHA_Y, TWEEN_DURATION, true);

            for (int i = 0; i < NormalGotchaAnimationSprites.Count; i++)
            {
                int index = i;
                
                CoroutineHelper.Delay(() =>
                {
                    if (NormalGotchaAnimationSprites.Count - 2 >= index)
                    {
                        NormalGotchaResult.DOText(NormalGotchaText[index], GOTCHA_RESULT_TWEEN_DURATION);
                    }
                    NormalGotchaImage.sprite = NormalGotchaAnimationSprites[index];
                }, index * GotchaAnimationInterval);
            }
            
            CoroutineHelper.Delay(() =>
            {
                GameManager.instance.GoldNumber -= NORMAL_GOTCHA_GOLD_COST;

                GotchaReward reward = GetRandom(NormalGotchaData.Datas);
            
                if (EqualityComparer<GotchaReward>.Default.Equals(reward, default))
                {
                    Debug.LogError("Random Error on gotcha. Check again");
                    return;
                }
            
                if (reward.MemoryNumber > 0 && reward.MemoryType != FriendName.None && !FriendCollectionManager.instance.CollectFinished(reward.MemoryType))
                {
                    FriendCollectionManager.instance.Collect(reward.MemoryType, reward.MemoryNumber);
                }

                if (reward.GoldNumber > 0)
                {
                    NormalGotchaResult.DOText($"와! {reward.GoldNumber}개의 <sprite=\"GameIcons2\" index=2>를 얻었다!", GOTCHA_RESULT_TWEEN_DURATION);
                } else if (reward.DiamondNumber > 0)
                {
                    NormalGotchaResult.DOText($"와! {reward.DiamondNumber}개의 <sprite=\"GameIcons2\" index=6>를 얻었다!", GOTCHA_RESULT_TWEEN_DURATION);
                }
                else
                {
                    NormalGotchaResult.DOText($"와! {reward.MemoryNumber}개의 {StringValue.GetStringValue(reward.MemoryType)}조각을 얻었다!", GOTCHA_RESULT_TWEEN_DURATION);
                }
                
                OnGotchaRewardGet?.Invoke(reward);
            
                Rebuild();
                GameManager.instance.SaveProgress();

                _graphicRaycaster.enabled = true;
            }, NormalGotchaAnimationSprites.Count * GotchaAnimationInterval);
        }
        
        private void DoSpecialGotcha()
        {
            _graphicRaycaster.enabled = false;
            
            Contents.DOLocalMoveY(Board.rect.yMax + SPECIAL_GOTCHA_Y, TWEEN_DURATION, true);

            for (int i = 0; i < SpecialGotchaAnimationSprites.Count; i++)
            {
                int index = i;
                
                CoroutineHelper.Delay(() =>
                {
                    if (SpecialGotchaAnimationSprites.Count - 2 >= index)
                    {
                        SpecialGotchaResult.DOText(SpecialGotchaText[index], GOTCHA_RESULT_TWEEN_DURATION);
                    }
                    SpecialGotchaImage.sprite = SpecialGotchaAnimationSprites[index];
                }, index * GotchaAnimationInterval);
            }

            CoroutineHelper.Delay(() =>
            {
                GameManager.instance.DiamondNumber -= SPECIAL_GOTCHA_DIAMOND_COST;

                GotchaReward reward = GetRandom(SpecialGotchaData.Datas);
            
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
            
                if (reward.GoldNumber > 0)
                {
                    SpecialGotchaResult.DOText($"와! {reward.GoldNumber}개의 골드를 얻었다!", GOTCHA_RESULT_TWEEN_DURATION);
                } else if (reward.DiamondNumber > 0)
                {
                    SpecialGotchaResult.DOText($"와! {reward.DiamondNumber}개의 다이아를 얻었다!", GOTCHA_RESULT_TWEEN_DURATION);
                }
                else
                {
                    SpecialGotchaResult.DOText($"와! {reward.MemoryNumber}개의 {StringValue.GetStringValue(reward.MemoryType)}조각을 얻었다!", GOTCHA_RESULT_TWEEN_DURATION);
                }
            
                OnGotchaRewardGet?.Invoke(reward);

                Rebuild();
                GameManager.instance.SaveProgress();

                _graphicRaycaster.enabled = true;
            }, SpecialGotchaAnimationSprites.Count * GotchaAnimationInterval);
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
                Contents.DOLocalMoveY(Board.rect.yMax + MEMORY_Y, TWEEN_DURATION, true);
                
                if (GameManager.instance.DiamondNumber < MemoryPriceList[((int)friendNameEnum) - 1])
                {
                    NotEnoughMoney();    
                    return;
                }
                

                if (FriendCollectionManager.instance.CollectFinished(friendNameEnum))
                {
                    NoticeText.text = "";
                    NoticeText.DOText(AlreadyCollectedText, NoticeTweenDuration);
                    return;
                }
                
                if (GameManager.ProgressSaveData.CollectionSellDict[friendNameEnum] >= PreloadedResources.instance.FriendSpecList[(int)friendNameEnum].MaxPurchasableNumber)
                {
                    NoticeText.text = "";
                    NoticeText.DOText(SellLimitExceedText, NoticeTweenDuration);
                    return;
                }

                ConfirmUI.Description.text = ConfirmDescription;
                ConfirmUI.AddConfirmListener(() => BuyMemory(friendNameEnum));
                ConfirmUI.Open();
            }
        }

        void BuyMemory(FriendName friendName)
        {
            MoonBunnyLog.print($"Memory Purchase {friendName}");
            
            NoticeText.text = "";
            NoticeText.DOText(PurchaseSuccess, NoticeTweenDuration);
            GameManager.instance.DiamondNumber -= MemoryPriceList[((int)friendName) - 1];

            GameManager.ProgressSaveData.CollectionSellDict[friendName]++;
            FriendCollectionManager.instance.Collect(friendName, 1);
            Rebuild();
            GameManager.instance.SaveProgress();
        }

        void NotEnoughMoney()
        {
            ConfirmUI.Description.text = NotEnoughMoneyDescription;
            ConfirmUI.Open();
        }
    }
}