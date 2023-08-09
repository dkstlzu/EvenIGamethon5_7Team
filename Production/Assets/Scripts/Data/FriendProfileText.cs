using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace MoonBunny
{
    [CreateAssetMenu(fileName = "FriendProfileText", menuName = "Stroy/FriendProfileText", order = 0)]
    public class FriendProfileText : ScriptableObject
    {
        [Multiline(10)] public string Description;
        public List<StringInt> MemoryTexts;
    }

    [Serializable]
    public class StringInt
    {
        [FormerlySerializedAs("Str")] [Multiline(2)]
        public string Text;
        [FormerlySerializedAs("Integer")] public int MemoryNumber;
        public Sprite StorySprite;
    }
}