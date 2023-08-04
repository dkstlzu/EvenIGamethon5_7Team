using System;
using System.Collections.Generic;
using DG.Tweening;
using dkstlzu.Utility;
using MoonBunny.Effects;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace MoonBunny.UIs
{
    public class SubLevelSelectUI : UI
    {
        [Serializable]
        public class SubLevelUI
        {
            public Image Image;
            public Image Text;
            public bool Enabled;
            public List<SubLevelSprites> SpriteList;
        }

        [Serializable]
        public class SubLevelSprites
        {
            public Sprite SubLevelSprite;
            public Sprite SubLevelTextSprite;
        }
        
        [Header("Sub Levels")]
        public RectTransform SubLevelContent;
        public float SubLevelSwipeTime;
        public List<SubLevelUI> SubLevelList;
        public Button StartButton;
        public Sprite LockStageSprite;
        
        [Header("BoostItem")]
        public List<BoostUI> BoostItemList;

        private StageName _stageName;
        private int _selectingLevel;
        private int _subLevelIndex;

        public void OnStageButtonClicked(StageName stageName)
        {
            _stageName = stageName;
            _selectingLevel = (int)stageName;
            
            for (int i = 0; i < SubLevelList.Count; i++)
            {
                SubLevelList[i].Enabled = GameManager.ProgressSaveData.ClearDict[(StageName)_selectingLevel] >= i;
                SubLevelList[i].Image.sprite = SubLevelList[i].Enabled ? SubLevelList[i].SpriteList[_selectingLevel].SubLevelSprite : LockStageSprite;
                SubLevelList[i].Text.sprite = SubLevelList[i].SpriteList[_selectingLevel].SubLevelTextSprite;
            }
        }
        
        public void OnLeftButtonClicked()
        {
            _subLevelIndex--;
            _subLevelIndex = Mathf.Clamp(_subLevelIndex, 0, 2);
            SubLevelContent.DOPivot(new Vector2(_subLevelIndex / 2f, 0.5f), SubLevelSwipeTime);
            StartButton.interactable = SubLevelList[_subLevelIndex].Enabled;
        }

        public void OnRightButtonClicked()
        {
            _subLevelIndex++;
            _subLevelIndex = Mathf.Clamp(_subLevelIndex, 0, 2);
            SubLevelContent.DOPivot(new Vector2(_subLevelIndex / 2f, 0.5f), SubLevelSwipeTime);
            StartButton.interactable = SubLevelList[_subLevelIndex].Enabled;
        }
        
        public void OnStartButtonClicked()
        {
            Stage.S_StageLevel = _selectingLevel;
            Stage.S_SubLevel = _subLevelIndex;
            
            SceneManager.LoadSceneAsync(StringValue.GetStringValue(_stageName, _subLevelIndex)).completed += (ao) =>
            {
                Stage stage = GameManager.instance.Stage;

                Character character = GameObject.FindWithTag("Player").GetComponent<Character>();

                foreach (BoostUI boost in BoostItemList)
                {
                    if (!boost.Checked) continue;
                    
                    switch (boost.BoostName)
                    {
                        case RocketBoostEffect.BoostName:
                            stage.BoostEffectList.Add(new RocketBoostEffect(character.Rigidbody));
                            break;
                        case MagnetBoostEffect.BoostName:
                            stage.BoostEffectList.Add(new MagnetBoostEffect(character));
                            break;
                        case StarCandyBoostEffect.BoostName:
                            stage.BoostEffectList.Add(new StarCandyBoostEffect(20, 0.5f));
                            break;
                        case DoubleGoldBoostEffect.BoostName:
                            stage.BoostEffectList.Add(new DoubleGoldBoostEffect(stage, 2));
                            break;
                    }
                }
                
                if (GameManager.instance.ShowTutorial)
                {
                    GameManager.instance.Stage.TutorialOn();
                }
            };
            
            GameManager.instance.GoldNumber -= BoostUI.S_ConsumingGold;
            BoostUI.S_ConsumingGold = 0;
            GameManager.instance.SaveProgress();
        }
    }
}