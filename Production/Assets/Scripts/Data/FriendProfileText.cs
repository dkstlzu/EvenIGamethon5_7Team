using System;
using System.Collections.Generic;
using UnityEngine;

namespace MoonBunny
{
    [CreateAssetMenu(fileName = "FriendProfileText", menuName = "Stroy/FriendProfileText", order = 0)]
    public class FriendProfileText : ScriptableObject
    {
        [Multiline(10)] public string Description;
        public List<StringInt> MemoryTexts;
        public Sprite StorySprite;
        
    }

    [Serializable]
    public class StringInt
    {
        [Multiline(2)]
        public string Str;
        public int Integer;
    }
}