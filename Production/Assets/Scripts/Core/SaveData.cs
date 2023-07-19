using System;
using UnityEngine;

namespace MoonBunny
{
    [Serializable]
    public class SaveData
    {
        public int FirstFriendCollectedNumber;
        public int SecondFriendCollectedNumber;
        public int ThirdFriendCollectedNumber;

        public int GoldNumber;

        public bool Stage1_1Clear;
        public bool Stage1_2Clear;
        public bool Stage1_3Clear;
        public bool Stage2_1Clear;
        public bool Stage2_2Clear;
        public bool Stage2_3Clear;
        public bool Stage3_1Clear;
        public bool Stage3_2Clear;
        public bool Stage3_3Clear;
        public bool Stage4_1Clear;
        public bool Stage4_2Clear;
        public bool Stage4_3Clear;
        public bool Stage5_1Clear;
        public bool Stage5_2Clear;
        public bool Stage5_3Clear;
    }
}