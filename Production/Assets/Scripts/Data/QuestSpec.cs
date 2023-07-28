using UnityEngine;

namespace MoonBunny
{
    [CreateAssetMenu(fileName = "QuestSpec", menuName = "Specs/QuestSpec", order = 0)]
    public class QuestSpec : ScriptableObject
    {
        public string DescriptionText;
    }
}