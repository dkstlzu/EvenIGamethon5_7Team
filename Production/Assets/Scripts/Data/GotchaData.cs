using System.Collections.Generic;
using MoonBunny.UIs;
using UnityEngine;

namespace MoonBunny
{
    [CreateAssetMenu(fileName = "Gotcha", menuName = "GotchaData", order = 0)]
    public class GotchaData : ScriptableObject
    {
        public List<GotchaReward> Datas;
    }
}