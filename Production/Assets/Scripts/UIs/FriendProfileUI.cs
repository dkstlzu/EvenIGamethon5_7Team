using System.Collections.Generic;
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
            bool isCollected = FriendCollectionManager.instance.Collection.Datas[(int)_selectingName].IsFinish();
            
            int index = (int)friendName;

            if (isCollected)
            {
                _horizontalSpeed.text = PreloadedResources.instance.FriendSpecList[index].HorizontalJumpSpeed.ToString();
                _bounciness.text = PreloadedResources.instance.FriendSpecList[index].VerticalJumpSpeed.ToString();
                _magneticPower.text = PreloadedResources.instance.FriendSpecList[index].MagneticPower.ToString();
                _specialAbility.text = PreloadedResources.instance.FriendSpecList[index].SpecialAbility;
                _profileImage.sprite = FriendSpriteList[index];
                _storyImage.sprite = FriendProfileTextList[index].StorySprite;
                _selectButton.interactable = true;
            }
            else
            {
                _horizontalSpeed.text = "???";
                _bounciness.text = "???";
                _magneticPower.text = "???";
                _specialAbility.text = "???";
                _profileImage.sprite = SilhouetteSprite;
                _selectButton.interactable = false;
            }
            
            _fullCollectionNumber = FriendCollectionManager.instance[friendName].TargetCollectingNumber;
            _currentCollectionNumber = FriendCollectionManager.instance[friendName].CurrentCollectingNumber;
            _collectionText.text = $"수집률 {_currentCollectionNumber} / {_fullCollectionNumber}";
            _collectionSlider.maxValue = _fullCollectionNumber;
            _collectionSlider.value = _currentCollectionNumber;
            
            _memoryImage.sprite = PieceOfMemorySpriteList[index];
            _description.text = FriendProfileTextList[index].Description;
            _memoryText.text = string.Empty;
            
            for (int i = 0; i < Mathf.Min(_currentCollectionNumber, FriendProfileTextList[index].MemoryTexts.Count); i++)
            {
                _memoryText.text += FriendProfileTextList[index].MemoryTexts[i] + "\n";
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