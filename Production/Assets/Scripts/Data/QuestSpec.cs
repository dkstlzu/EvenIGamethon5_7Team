using UnityEngine;

namespace MoonBunny
{
    [CreateAssetMenu(fileName = "QuestSpec", menuName = "Specs/QuestSpec", order = 0)]
    public class QuestSpec : ScriptableObject
    {
        public int Id;
        public int TargetProgress;
        public int DependentId = -1;
        public bool Repeatable;
        [Multiline(5)]
        public string DescriptionText;
        [Multiline(5)]
        public string DescriptionTextOnDisabled = "???????";
        [Multiline(5)]
        public string DescriptionTextOnHidden = "???????";

        public QuestReward Reward;
    }
}