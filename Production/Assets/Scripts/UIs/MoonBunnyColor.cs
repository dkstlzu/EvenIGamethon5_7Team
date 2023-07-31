using System;
using UnityEngine;

namespace MoonBunny.UIs
{
    public class MoonBunnyColor
    {
        public static Color IconGaugeFull = FromCode("c0f528");
        public static Color BackgroundColor = FromCode("000000", 230);
        public static Color DisabledColor = FromCode("ffffff", 150);
        public static Color QuestCompleteColor = Color.yellow;
        public static Color QuestHiddenColor = Color.grey;
        public static Color QuestDisabledColor = Color.grey;
        
        private static Color FromCode(string code, int alpha = 255)
        {
            return new Color32((byte)Convert.ToInt32(code.Substring(0, 2), 16), (byte)Convert.ToInt32(code.Substring(2, 2), 16), (byte)Convert.ToInt32(code.Substring(4, 2), 16), (byte)(alpha));
        }
    }
}