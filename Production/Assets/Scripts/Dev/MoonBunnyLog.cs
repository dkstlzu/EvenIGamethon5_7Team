using System.Text;
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

        public static void print(object msg, string tag)
        {
            Debug.Log($"[{tag}] : {msg}");
        }

        public static void printByteArray(byte[] byteArr)
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.AppendLine(Prefix + "ByteArray");

            for (int i = 0; i < byteArr.Length; i++)
            {
                stringBuilder.Append(byteArr[i] + " ");

                if (i % 4 == 3)
                {
                    stringBuilder.AppendLine();
                }
            }
            
            Debug.Log(stringBuilder);
        }
    }
}