using System;
using UnityEngine;

namespace MoonBunny
{
    [System.Serializable]
    public class SaveData
    {
        public int FirstFriendCollectedNumber;
        public int SecondFriendCollectedNumber;
        public int ThirdFriendCollectedNumber;

        public bool Stage1Clear;
        public bool Stage2Clear;
        public bool Stage3Clear;
        public bool Stage4Clear;
        public bool Stage5Clear;
    }
}