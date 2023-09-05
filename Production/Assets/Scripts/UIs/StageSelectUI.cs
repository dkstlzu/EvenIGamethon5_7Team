using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace MoonBunny.UIs
{
    public class StageSelectUI : UI
    {
        [Serializable]
        public class StageButtonUI
        {
            public Button Button;
            public Image LockImage;
            public Image StarImage;
        }
        
        public List<StageButtonUI> StageButtonList;
        public List<Sprite> StarSpriteList;
        public SubLevelSelectUI SubLevelSelectUI;

        protected override void Awake()
        {
            base.Awake();
            
            GameManager.instance.SaveLoadSystem.OnSaveDataLoaded += () =>
            {
                for (int i = 0; i < StageButtonList.Count; i++)
                {
                    int subLevelClear = GameManager.SaveData.ClearDict[(StageName)i];
                    if (subLevelClear >= 0)
                    StageButtonList[i].StarImage.sprite = StarSpriteList[subLevelClear];
                    StageButtonList[i].Button.interactable = subLevelClear >= 0;
                    StageButtonList[i].LockImage.enabled = subLevelClear < 0;
                }
            };
        }
                
        public void OnStageButtonClicked(string stageNameStr)
        {
            StageName stageName;
            if (!StageName.TryParse(stageNameStr, true, out stageName))
            {
                Debug.LogError("StageName is wrong while stage button clicked");
                return;
            }

            SubLevelSelectUI.OnStageButtonClicked(stageName);
            SubLevelSelectUI.Open();
        }
    }
}