using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MoonBunny.Dev.Editor
{
    public class SaveMapData
    {
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

        private const string SCENE_FOLDER_PATH = "Assets/Scenes/";
            
        [MenuItem("Dev/MapData/Load All", priority = 3)]
        public static void LoadDataAll()
        {
            string[] targetScenes = SceneName.StageNames;
            
            foreach (string targetScene in targetScenes)
            {
                if (targetScene == SceneName.StageChallenge) continue;
                
                EditorSceneManager.OpenScene(SCENE_FOLDER_PATH + targetScene + ".unity");
                LoadData();
            
                Scene currentScene = EditorSceneManager.GetActiveScene();
                string path = currentScene.path;
                bool saveOK = EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene(), path);
                Debug.Log($"Saved Scene {currentScene.name} " + (saveOK ? "OK" : "Error!"));
            }
        }
        
        [MenuItem("Dev/MapData/Load", priority = 1)]
        public static void LoadData()
        {
            ClearMapData();
            
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
                if (gimmicks[i] is GameOverFloor || gimmicks[i] is StageGoal) continue;
                
                if (gimmicks[i] == null) continue;
                
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