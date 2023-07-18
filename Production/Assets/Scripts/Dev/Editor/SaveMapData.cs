using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MoonBunny.Dev.Editor
{
    public class SaveMapData
    {
        [MenuItem("Dev/MapData/Save", priority = 0)]
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
        
        [MenuItem("Dev/MapData/Load", priority = 1)]
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

        [MenuItem("Dev/MapData/Clear", priority = 2)]
        public static void ClearMapData()
        {
            Gimmick[] gimmicks = GameObject.FindObjectsByType<Gimmick>(FindObjectsInactive.Include, FindObjectsSortMode.None);

            for (int i = 0; i < gimmicks.Length; i++)
            {
                if (gimmicks[i] is GameOverFloor) continue;
                
                MonoBehaviour.DestroyImmediate(gimmicks[i].gameObject);
            }
        }

        private static string GetFileName()
        {
            string activeSceneName = SceneManager.GetActiveScene().name;
            string fileName = activeSceneName + "MapData";

            return fileName;
        }
    }
}