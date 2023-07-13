using System;
using UnityEngine;

namespace MoonBunny.UIs
{
    public class MoonBunnyColor
    {
        public static Color IconGaugeFull = FromCode("c0f528");
        
        private static Color FromCode(string code, int alpha = 1)
        {
            return new Color32((byte)Convert.ToInt32(code.Substring(0, 2), 16), (byte)Convert.ToInt32(code.Substring(2, 2), 16), (byte)Convert.ToInt32(code.Substring(4, 2), 16), (byte)(alpha * 255));
        }
    }
}