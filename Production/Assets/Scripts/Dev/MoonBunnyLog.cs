using UnityEngine;

namespace MoonBunny.Dev
{
    public class MoonBunnyLog
    {
        private const string Prefix = "[MoonBunnyLog] : ";

        public static void print(object msg)
        {
            Debug.Log(Prefix + msg.ToString());
        }
        
        public static void print(object msg, int lineOffset)
        {
            string lineOffetStr = string.Empty;

            for (int i = 0; i < lineOffset; i++)
            {
                lineOffetStr += "\n";
            }
            
            Debug.Log(Prefix + lineOffetStr + msg.ToString());
        }
    }
}