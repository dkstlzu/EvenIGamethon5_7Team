using UnityEngine;
using EvenI7.ProtoScreenSplit;
using TMPro;
using UnityEngine.SceneManagement;

namespace EvenI7.ProtoScreenSplitLying
{
    public class UIUpdater : MonoBehaviour
    {
        public TextMeshProUGUI ScoreText;
        public TextMeshProUGUI FirstFriendText;
        public TextMeshProUGUI SecondFriendText;
        public TextMeshProUGUI ThirdFriendText;

        private readonly string _scoreTextPrefix = "Score : ";
        private readonly string _firstFriendTextPrefix = "FirstFriend : ";
        private readonly string _secondFriendTextPrefix = "SecondFriend : ";
        private readonly string _thirdFriendTextPrefix = "ThirdFriend : ";
        
        private readonly string _scoreTextPostfix = "";
        private readonly string _firstFriendTextPostfix = "/5";
        private readonly string _secondFriendTextPostfix = "/10";
        private readonly string _thirdFriendTextPostfix = "/20";

        private int _score;

        private void Awake()
        {
            FriendCollectionManager.instance.OnCollectFriend += FriendUIUpdate;
            FriendCollectionManager.instance.OnCollectFriendFinish += (name) =>
            {
                print($"{name} Friend Collect finish");
            };

            Item.OnItemTaken += ScoreUIUpdate;
        }

        private void FriendUIUpdate(FriendName name, int number)
        {
            switch (name)
            {
                case FriendName.None:
                    break;
                case FriendName.First:
                    FirstFriendText.text = _firstFriendTextPrefix + number + _firstFriendTextPostfix;
                    break;
                case FriendName.Second:
                    SecondFriendText.text = _secondFriendTextPrefix + number + _secondFriendTextPostfix;
                    break;
                case FriendName.Third:
                    ThirdFriendText.text = _thirdFriendTextPrefix + number + _thirdFriendTextPostfix;
                    break;
            }
        }

        private void ScoreUIUpdate(int score)
        {
            _score += score;
            ScoreText.text = _scoreTextPrefix + _score + _scoreTextPostfix;
        }

        public void Restart()
        {
            SceneManager.LoadScene("ProtoScreenSplitLying");
        }
    }
}