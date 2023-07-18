using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MoonBunny.Dev.Editor
{
    public class SaveMapData
    {
        [MenuItem("Dev/SaveMapData", priority = 1)]
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
        
        [MenuItem("Dev/LoadMapData", priority = 1)]
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
            string fileName = activeSceneName + "MapData";

            return fileName;
        }
    }
}