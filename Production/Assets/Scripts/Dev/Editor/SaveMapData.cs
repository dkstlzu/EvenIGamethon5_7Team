using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MoonBunny.Dev.Editor
{
    public class SaveMapData
    {
        [MenuItem("Dev/SaveMapData")]
        public static void SaveData()
        {
            string fileName = GetFileName();
            if (fileName == string.Empty)
            {
                Debug.LogWarning("WrongScene");
                return;
            }
            
            SaveLoadSystem system = new SaveLoadSystem("MapData", fileName, "csv");
            
            system.SaveCSV();
        }
        
        [MenuItem("Dev/LoadMapData")]
        public static void LoadData()
        {
            string fileName = GetFileName();
            if (fileName == string.Empty)
            {
                Debug.LogWarning("WrongScene");
                return;
            }
            
            SaveLoadSystem system = new SaveLoadSystem("MapData", fileName, "csv");
            
            system.LoadCSV();
        }

        private static string GetFileName()
        {
            string activeSceneName = SceneManager.GetActiveScene().name;
            string fileName = "";

            switch (activeSceneName)
            {
                case SceneName.Stage1:
                    fileName = "Stage1Map";
                    break;
                case SceneName.Stage2:
                    fileName = "Stage2Map";
                    break;
                case SceneName.Stage3:
                    fileName = "Stage3Map";
                    break;
                case SceneName.Stage4:
                    fileName = "Stage4Map";
                    break;
                case SceneName.Stage5:
                    fileName = "Stage5Map";
                    break;
                case SceneName.StageChallenge:
                    fileName = "StageChallengeMap";
                    break;
            }

            return fileName;
        }
    }
}