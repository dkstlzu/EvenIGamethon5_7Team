using System;
using System.Collections.Generic;
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
            public Image StarImage;
            public Image LockImage;
        }
        
        public List<StageButtonUI> StageButtonList;
        public List<Sprite> StarSpriteList;
        public SubLevelSelectUI SubLevelSelectUI;

        private void Awake()
        {
            for (int i = 0; i < StageButtonList.Count; i++)
            {
                int subLevelClear = GameManager.instance.ClearDict[(StageName)i];
                StageButtonList[i].StarImage.sprite = StarSpriteList[subLevelClear];
                
                if (i+1 < StageButtonList.Count)
                {
                    StageButtonList[i+1].Button.interactable = subLevelClear > 2;
                    StageButtonList[i+1].LockImage.enabled = subLevelClear <= 2;
                }
            }
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