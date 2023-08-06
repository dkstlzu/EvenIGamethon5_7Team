using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using dkstlzu.Utility;
using MoonBunny;
using MoonBunny.UIs;
using UnityEditor;
using UnityEngine;

namespace Dev.Editor
{
    public static class GotchaSimulator
    {
        private const string GOTCHA_DATA_PATH = "Assets/Resources/Specs/";
        private static string GOTCHA_RESULT_PATH = Path.Combine(Application.persistentDataPath, "Test");

        [MenuItem("Dev/Gotcha/Open Directory")]
        static void OpenGotchaDirectory()
        {
            System.Diagnostics.Process.Start(GOTCHA_RESULT_PATH);
        }

        [MenuItem("Dev/Gotcha/Normal 1000")]
        static void NormalGotcha1000()
        {
            GotchaData normalGotcha = AssetDatabase.LoadAssetAtPath<GotchaData>(GOTCHA_DATA_PATH + "NormalGotcha.asset");
            float randomValue;
            int index;
            int totalGold = 0, totalDiamond = 0;
            List<int> memoryNumbers = new List<int>();
            Dictionary<int, int> indexDict = new Dictionary<int, int>();

            foreach (var friendName in Enum.GetValues(typeof(FriendName)))
            {
                if ((int)friendName < 0) continue;
                memoryNumbers.Add(0);
            }

            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < 1000; i++)
            {
                GotchaReward reward = GetRandom(normalGotcha.Datas, out randomValue, out index);

                totalGold += reward.GoldNumber;
                totalDiamond += reward.DiamondNumber;

                if ((int)reward.MemoryType >= 0)
                {
                    memoryNumbers[(int)reward.MemoryType] += reward.MemoryNumber;
                }

                if (!indexDict.ContainsKey(index))
                {
                    indexDict.Add(index, 0);
                }

                indexDict[index]++;
                
                stringBuilder.AppendLine($"NormalGotcha : {reward} with {randomValue} at {index}");
            }

            StringBuilder summaryBuilder = new StringBuilder();
            summaryBuilder.AppendLine($"Summary : TotalGold {totalGold}, TotalDiamond {totalDiamond}");

            summaryBuilder.AppendLine();
            for (int i = 0; i < memoryNumbers.Count; i++)
            {
                summaryBuilder.AppendLine($"{(FriendName)i} : {memoryNumbers[i]}");
            }
            summaryBuilder.AppendLine();

            var ordered = indexDict.OrderBy(pair => pair.Key);
            var enumerator = ordered.GetEnumerator();

            while (enumerator.MoveNext())
            {
                summaryBuilder.AppendLine($"Picked Index : {enumerator.Current.Key} - {enumerator.Current.Value}");
            }
            summaryBuilder.AppendLine();

            summaryBuilder.Append(stringBuilder);
            
            Debug.Log(summaryBuilder);
            SaveLoadSystem saveLoadSystem = new SaveLoadSystem("Test", "NormalGotcha", "txt");
            saveLoadSystem.SaveString(summaryBuilder.ToString());
            
            OpenGotchaDirectory();
        }
        
        [MenuItem("Dev/Gotcha/Special 1000")]
        static void SpecialGotcha1000()
        {
            GotchaData specialGotcha = AssetDatabase.LoadAssetAtPath<GotchaData>(GOTCHA_DATA_PATH + "SpecialGotcha.asset");
            float randomValue;
            int index;
            int totalGold = 0, totalDiamond = 0;
            List<int> memoryNumbers = new List<int>();
            Dictionary<int, int> indexDict = new Dictionary<int, int>();
            
            foreach (var friendName in Enum.GetValues(typeof(FriendName)))
            {
                if ((int)friendName < 0) continue;
                memoryNumbers.Add(0);
            }
            
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < 1000; i++)
            {
                GotchaReward reward = GetRandom(specialGotcha.Datas, out randomValue, out index);

                totalGold += reward.GoldNumber;
                totalDiamond += reward.DiamondNumber;

                if ((int)reward.MemoryType >= 0)
                {
                    memoryNumbers[(int)reward.MemoryType] += reward.MemoryNumber;
                }

                if (!indexDict.ContainsKey(index))
                {
                    indexDict.Add(index, 0);
                }

                indexDict[index]++;
                
                stringBuilder.AppendLine($"SpecialGotcha : {reward} with {randomValue} at {index}");
            }
            
            StringBuilder summaryBuilder = new StringBuilder();
            summaryBuilder.AppendLine($"Summary : TotalGold {totalGold}, TotalDiamond {totalDiamond}");

            summaryBuilder.AppendLine();
            for (int i = 0; i < memoryNumbers.Count; i++)
            {
                summaryBuilder.AppendLine($"{(FriendName)i} : {memoryNumbers[i]}");
            }
            summaryBuilder.AppendLine();

            var ordered = indexDict.OrderBy(pair => pair.Key);
            var enumerator = ordered.GetEnumerator();

            while (enumerator.MoveNext())
            {
                summaryBuilder.AppendLine($"Picked Index : {enumerator.Current.Key} - {enumerator.Current.Value}");
            }
            summaryBuilder.AppendLine();

            summaryBuilder.Append(stringBuilder);
            
            Debug.Log(summaryBuilder);
            SaveLoadSystem saveLoadSystem = new SaveLoadSystem("Test", "SpecialGotcha", "txt");
            saveLoadSystem.SaveString(summaryBuilder.ToString());
            
            OpenGotchaDirectory();
        }
        
        static GotchaReward GetRandom(IEnumerable<GotchaReward> rewards, out float randomValue, out int index)
        {
            float total = 0;
            List<float> potentialCumulativeSum = new List<float>();

            using (IEnumerator<GotchaReward> enumerator = rewards.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    total += enumerator.Current.Potential;
                    potentialCumulativeSum.Add(total);
                }
            }

            randomValue = UnityEngine.Random.Range(0, total);
            
            using (IEnumerator<GotchaReward> enumerator = rewards.GetEnumerator())
            {
                index = 0;
                while (enumerator.MoveNext())
                {
                    if (randomValue <= potentialCumulativeSum[index])
                    {
                        return enumerator.Current;
                    }

                    index++;
                }
            }

            return default;
        }
    }
}