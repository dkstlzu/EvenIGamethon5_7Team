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
        public string DescriptionText;
        public string DescriptionTextOnDisabled = "???????";
        public string DescriptionTextOnHidden = "???????";

        public QuestReward Reward;
    }
}