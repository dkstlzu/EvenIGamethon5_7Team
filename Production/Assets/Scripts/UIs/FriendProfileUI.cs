using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MoonBunny.UIs
{
    public class FriendProfileUI : MonoBehaviour
    {
        public List<FriendSpec> FriendSpecList;
        public List<Sprite> FriendSpriteList;
        public List<Sprite> PieceOfMemorySpriteList;
        public List<FriendProfileText> FriendProfileTextList;

        private int _fullCollectionNumber;
        private int _currentCollectionNumber;

        public StartSceneUI StartSceneUI;
        public CanvasGroup CanvasGroup;
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
            return;

            _selectingName = friendName;
            int index = (int)friendName;

            _horizontalSpeed.text = FriendSpecList[index].HorizontalJumpSpeed.ToString();
            _bounciness.text = FriendSpecList[index].VerticalJumpSpeed.ToString();
            _magneticPower.text = FriendSpecList[index].MagneticPower.ToString();
            
            _fullCollectionNumber = FriendCollectionManager.instance[friendName].TargetCollectingNumber;
            _currentCollectionNumber = FriendCollectionManager.instance[friendName].CurrentCollectingNumber;
            _collectionText.text = $"수집률 {_currentCollectionNumber} / {_fullCollectionNumber}";
            _collectionSlider.maxValue = _fullCollectionNumber;
            _collectionSlider.value = _currentCollectionNumber;

            _profileImage.sprite = FriendSpriteList[index];
            _memoryImage.sprite = PieceOfMemorySpriteList[index];
            _description.text = FriendProfileTextList[index].Description;
            _memoryText.text = string.Empty;
            for (int i = 0; i < _currentCollectionNumber; i++)
            {
                _memoryText.text += FriendProfileTextList[index].MemoryTexts[i] + "\n";
            }
            _storyText.text = FriendProfileTextList[index].StoryText;
        }

        public void Open()
        {
            CanvasGroup.DOFade(1, 1);
            CanvasGroup.blocksRaycasts = true;
        }

        public void OnExitButtonClicked()
        {
            CanvasGroup.DOFade(0, 1);
            CanvasGroup.blocksRaycasts = false;
            _content.DOPivot(new Vector2(0.5f, 0.5f), 1);
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
            StartSceneUI.CurrentSelectedFriendName = _selectingName;
            StartSceneUI.CurrentProfileImage.sprite = StartSceneUI.FriendProfileSpriteList[(int)_selectingName];
        }
    }
}