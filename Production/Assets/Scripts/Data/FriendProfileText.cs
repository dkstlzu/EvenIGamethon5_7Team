using System.Collections.Generic;
using UnityEngine;

namespace MoonBunny
{
    [CreateAssetMenu(fileName = "FriendProfileText", menuName = "Stroy/FriendProfileText", order = 0)]
    public class FriendProfileText : ScriptableObject
    {
        [Multiline(10)] public string Description;
        [Multiline(10)] public List<string> MemoryTexts;
        [Multiline(10)] public string StoryText;
    }
}