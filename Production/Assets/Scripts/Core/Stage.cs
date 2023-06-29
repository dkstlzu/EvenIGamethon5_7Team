using dkstlzu.Utility;
using UnityEngine;

namespace MoonBunny
{
    public enum StageName
    {
        [StringValue(SceneName.Stage1)]
        One,
        [StringValue(SceneName.Stage2)]
        Two,
        [StringValue(SceneName.Stage3)]
        Three,
        [StringValue(SceneName.Stage4)]
        Four,
        [StringValue(SceneName.Stage5)]
        Five,
        [StringValue(SceneName.StageChallenge)]
        Challenge,
    }
    public class Stage : MonoBehaviour
    {
        public StageName Name;
    }
}