using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MoonBunny.UIs
{
    public class FriendProfileUI : UI
    {
        public List<FriendSpec> FriendSpecList;
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
        [SerializeField] private TextMeshProUGUI _description;
        [SerializeField] private TextMeshProUGUI _memoryText;
        [SerializeField] private TextMeshProUGUI _storyText;
        [SerializeField] private TextMeshProUGUI _collectionText;
        [SerializeField] private Slider _collectionSlider;
        [SerializeField] private Image _profileImage; 
        [SerializeField] private Image _memoryImage; 
        [SerializeField] private RectTransform _content; 
        
        public void Set(FriendName friendName)
        {
            _selectingName = friendName;
            bool isCollected = FriendCollectionManager.instance.Collection.Datas[(int)_selectingName].IsFinish();
            
            int index = (int)friendName;

            if (isCollected)
            {
                _horizontalSpeed.text = FriendSpecList[index].HorizontalJumpSpeed.ToString();
                _bounciness.text = FriendSpecList[index].VerticalJumpSpeed.ToString();
                _magneticPower.text = FriendSpecList[index].MagneticPower.ToString();
            }
            else
            {
                _horizontalSpeed.text = "???";
                _bounciness.text = "???";
                _magneticPower.text = "???";
            }

            
            _fullCollectionNumber = FriendCollectionManager.instance[friendName].TargetCollectingNumber;
            _currentCollectionNumber = FriendCollectionManager.instance[friendName].CurrentCollectingNumber;
            _collectionText.text = $"수집률 {_currentCollectionNumber} / {_fullCollectionNumber}";
            _collectionSlider.maxValue = _fullCollectionNumber;
            _collectionSlider.value = _currentCollectionNumber;

            if (isCollected)
            {
                _profileImage.sprite = FriendSpriteList[index];
            }
            else
            {
                _profileImage.sprite = SilhouetteSprite;
            }
            
            _memoryImage.sprite = PieceOfMemorySpriteList[index];
            _description.text = FriendProfileTextList[index].Description;
            _memoryText.text = string.Empty;
            
            for (int i = 0; i < Mathf.Min(_currentCollectionNumber, FriendProfileTextList[index].MemoryTexts.Count); i++)
            {
                _memoryText.text += FriendProfileTextList[index].MemoryTexts[i] + "\n";
            }

            if (isCollected)
            {
                _storyText.text = FriendProfileTextList[index].StoryText;
            }
            else
            {
                _storyText.text = "날 구해줘! 부탁해";
            }

        }

        public void OnLeftButtonClicked()
        {
            float xTarget = Mathf.Clamp01(_content.pivot.x - 0.5f);
            _content.DOPivot(new Vector2(xTarget, 0f), 1);
        }
        
        public void OnRightButtonClicked()
        {
            float xTarget = Mathf.Clamp01(_content.pivot.x + 0.5f);
            _content.DOPivot(new Vector2(xTarget, 0.5f), 1);
        }

        public void OnConfirmButtonClicked()
        {
            GameManager.instance.UsingFriendName = _selectingName;
            StartSceneUI.ProfileImage.sprite = StartSceneUI.FriendSelectUI.FriendLibraryUIList[(int)_selectingName].ProfileSprite;
            OnExitButtonClicked();
        }
    }
}