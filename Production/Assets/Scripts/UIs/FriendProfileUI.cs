using System.Collections.Generic;
using System.Text;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MoonBunny.UIs
{
    public class FriendProfileUI : UI
    {
        public List<Sprite> FriendSpriteList;
        public List<Sprite> PieceOfMemorySpriteList;
        public List<FriendProfileText> FriendProfileTextList;
        public Sprite SilhouetteSprite;
        public Sprite UnCollectedStorySprite;

        private int _fullCollectionNumber;
        private int _currentCollectionNumber;

        public StartSceneUI StartSceneUI;
        private FriendName _selectingName;
        
        [Header("References")]
        [SerializeField] private TextMeshProUGUI _horizontalSpeed;
        [SerializeField] private TextMeshProUGUI _bounciness;
        [SerializeField] private TextMeshProUGUI _magneticPower;
        [SerializeField] private TextMeshProUGUI _specialAbility;
        [SerializeField] private TextMeshProUGUI _description;
        [SerializeField] private TextMeshProUGUI _memoryText;
        [SerializeField] private Image _storyImage;
        [SerializeField] private TextMeshProUGUI _collectionText;
        [SerializeField] private Slider _collectionSlider;
        [SerializeField] private Image _profileImage; 
        [SerializeField] private Image _memoryImage; 
        [SerializeField] private RectTransform _content;
        [SerializeField] private Button _selectButton; 
        
        public void Set(FriendName friendName)
        {
            _selectingName = friendName;
            FriendCollection.Data targetData = FriendCollectionManager.instance.Collection.Datas[(int)_selectingName];
            bool isCollected = targetData.IsFinish();
            
            int index = (int)friendName;

            FriendSpec targetSpec = PreloadedResources.instance.FriendSpecList[index];
            
            _profileImage.sprite = FriendSpriteList[index];

            if (isCollected)
            {
                _horizontalSpeed.text = targetSpec.HorizontalJumpSpeed.ToString();
                _bounciness.text = targetSpec.VerticalJumpSpeed.ToString();
                _magneticPower.text = targetSpec.MagneticPower.ToString();
                _specialAbility.text = targetSpec.SpecialAbility;
                _profileImage.color = Color.white;
                _selectButton.interactable = true;
            }
            else
            {
                _horizontalSpeed.text = "???";
                _bounciness.text = "???";
                _magneticPower.text = "???";
                _specialAbility.text = "???";
                _storyImage.sprite = UnCollectedStorySprite;
                float colorValue = StartSceneUI.FriendSelectUI.FriendLibraryImageCurve.Evaluate(targetData.GetPercent());
                _profileImage.color = new Color(colorValue, colorValue, colorValue, 1);
                _selectButton.interactable = false;
            }
            
            _memoryImage.sprite = PieceOfMemorySpriteList[index];
            _fullCollectionNumber = FriendCollectionManager.instance[friendName].TargetCollectingNumber;
            _currentCollectionNumber = FriendCollectionManager.instance[friendName].CurrentCollectingNumber;
            _collectionText.text = $"수집률 {_currentCollectionNumber} / {_fullCollectionNumber}";
            _collectionSlider.maxValue = _fullCollectionNumber;
            _collectionSlider.value = _currentCollectionNumber;
            
            FriendProfileText profileText = FriendProfileTextList[index];
            _description.text = profileText.Description;
            _memoryText.text = string.Empty;

            StringBuilder stringBuilder = new StringBuilder();

            int memoryIndex = -1;
            
            for (int i = 0; i < profileText.MemoryTexts.Count; i++)
            {
                if (_currentCollectionNumber >= profileText.MemoryTexts[i].MemoryNumber)
                {
                    stringBuilder.AppendLine($"{i}. {profileText.MemoryTexts[i].Text}");
                    memoryIndex++;
                }
                else
                {
                    stringBuilder.AppendLine($"{i}. ????????");
                }
            }

            _memoryText.text = stringBuilder.ToString();
            if (memoryIndex >= 0)
            {
                _storyImage.sprite = profileText.MemoryTexts[memoryIndex].StorySprite;
            }
        }

        private float targetPivotX = 0;
        public void OnLeftButtonClicked()
        {
            targetPivotX = Mathf.Clamp01(targetPivotX - 0.5f);
            _content.DOPivot(new Vector2(targetPivotX, 0f), 1);
        }
        
        public void OnRightButtonClicked()
        {
            targetPivotX = Mathf.Clamp01(targetPivotX + 0.5f);
            _content.DOPivot(new Vector2(targetPivotX, 0.5f), 1);
        }

        public void OnConfirmButtonClicked()
        {
            GameManager.instance.UsingFriendName = _selectingName;
            StartSceneUI.ProfileImage1.sprite = StartSceneUI.FriendSelectUI.FriendLibraryUIList[(int)_selectingName].ProfileSprite;
            StartSceneUI.ProfileImage2.sprite = StartSceneUI.FriendSelectUI.FriendLibraryUIList[(int)_selectingName].ProfileSprite;
            OnExitButtonClicked();
        }
    }
}