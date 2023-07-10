using System;
using System.IO;
using System.Text;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace MoonBunny
{
    public class SaveLoadSystem
    {
        public bool DataIsLoaded;
        public SaveData SaveData;
        public string DataSavingFolderPath = Path.Combine(Application.streamingAssetsPath, "Saves");
        private string DataSavingFileName = "Save";
        private string DataSavingExtension = ".txt";

        public string SaveDataFilePath => Path.Combine(DataSavingFolderPath, DataSavingFileName) + DataSavingExtension;

        public SaveLoadSystem()
        {
        }

        public SaveLoadSystem(string folder, string fileName, string extension)
        {
            DataSavingFolderPath = Path.Combine(Application.streamingAssetsPath, folder);
            DataSavingFileName = fileName;
            DataSavingExtension = "." + extension;
        }

#if UNITY_EDITOR
        [MenuItem("Dev/CreateDefaultSaveData")]
        public static void CreateDefaultSaveDataFile()
        {
            SaveData data = new SaveData();
            string jsonData = JsonUtility.ToJson(data, true);
            byte[] byteData = Encoding.UTF8.GetBytes(jsonData);

            string defaultPath = Path.Combine(Application.streamingAssetsPath, "Saves", "Save") + ".txt";
            
            File.WriteAllText(defaultPath, String.Empty);

            using (FileStream fs = File.OpenWrite(defaultPath))
            {
                fs.Write(byteData);
                fs.Flush();
            }
        }
#endif
            
        public void SaveJson(object data)
        {
            string jsonData = JsonUtility.ToJson(data, true);
            byte[] byteData = Encoding.UTF8.GetBytes(jsonData);
            
            File.WriteAllText(SaveDataFilePath, String.Empty);

            using (FileStream fs = File.OpenWrite(SaveDataFilePath))
            {
                fs.Write(byteData);
                fs.Flush();
            }
        }

        public void SaveDatabase()
        {
            SaveJson(SaveData);
        }

        public object LoadJson(Type type)
        {
            using (FileStream fs = File.OpenRead(SaveDataFilePath))
            {
                byte[] data = new byte[fs.Length];
                string jsonData = String.Empty;
                int count;
                count = fs.Read(data, 0, data.Length);
                
                if (count > 0)
                {
                    jsonData = Encoding.UTF8.GetString(data, 0, count);
                    return Convert.ChangeType(JsonUtility.FromJson(jsonData, type), type);
                }
            }

            return null;
        }

        public T LoadJson<T>()
        {
            return (T)LoadJson(typeof(T));
        }

        public void LoadDatabase()
        {
            DataIsLoaded = true;
            SaveData = LoadJson<SaveData>();
        }

        private static string CSVSeperator = ",";

        private static string[] CSVHeader = new string[]
        {
            "No",
            "X",
            "Y",
            "SizeX",
            "SizeY",
            "BoundcyPower"
        };

        [MenuItem("Dev/CreateDefaultMapData")]
        public static void CreateDefaultMapData()
        {
            string directory = Path.Combine(Application.streamingAssetsPath, "MapData");
            
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            string file = Path.Combine(Application.streamingAssetsPath, "MapData", "Map.csv");
            
            using (var writer = File.CreateText(file))
            {
                StringBuilder str = new StringBuilder();
                str.AppendJoin(CSVSeperator, CSVHeader);
                writer.WriteLine(str);
            }
        }

        public void SaveCSV()
        {
            BouncyPlatform[] platforms = GameObject.FindObjectsByType<BouncyPlatform>(FindObjectsInactive.Include, FindObjectsSortMode.None);

            StringBuilder[] str = new StringBuilder[platforms.Length];
            string[] fileStr = new string[platforms.Length];
            
            for (int i = 0; i < platforms.Length; i++)
            {
                str[i] = new StringBuilder();
                string[] values = new[] {i.ToString(), platforms[i].transform.position.x.ToString(), platforms[i].transform.position.y.ToString(), 
                    platforms[i].transform.localScale.x.ToString(), platforms[i].transform.localScale.y.ToString(), platforms[i].JumpPower.ToString()};
                str[i].AppendJoin(CSVSeperator, values);
                fileStr[i] = str[i].ToString();
            }

            CreateDefaultMapData();
            File.AppendAllLines(SaveDataFilePath, fileStr);
        }

        public void LoadCSV()
        {
            GameObject platformsParent = GameObject.FindWithTag("Platforms");

            if (platformsParent == null)
            {
                Debug.LogError("There is no Platforms taged Gameobject in active scene check again");
                return;
            }

            string platformAssetPath = "Assets/Prefabs/Level/BouncyPlatform.prefab";
            GameObject platformPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(platformAssetPath);

            if (platformPrefab == null)
            {
                Debug.LogError($"There is no platform prefab on {platformAssetPath} check again");
                return;
            }

            BouncyPlatform[] platformsInSceneAlready = GameObject.FindObjectsByType<BouncyPlatform>(FindObjectsInactive.Include, FindObjectsSortMode.None);

            foreach (BouncyPlatform platform in platformsInSceneAlready)
            {
                MonoBehaviour.DestroyImmediate(platform.gameObject);
            }
            
            string[] fileContents = File.ReadAllLines(SaveDataFilePath);

            int xIndex = Array.IndexOf(CSVHeader, "X");
            int yIndex = Array.IndexOf(CSVHeader, "Y");
            int sizeXIndex = Array.IndexOf(CSVHeader, "SizeX");
            int sizeYIndex = Array.IndexOf(CSVHeader, "SizeY");
            int bouncyPowerIndex = Array.IndexOf(CSVHeader, "SizeX");

            GameObject[] platformGos = new GameObject[fileContents.Length-1];

            for (int i = 1; i < fileContents.Length; i++)
            {
                string[] lineContent = fileContents[i].Split(CSVSeperator);

                Vector3 platformPosition = new Vector3( float.Parse(lineContent[xIndex]), float.Parse(lineContent[yIndex]), 0);
                Vector3 platformScale = new Vector3(float.Parse(lineContent[sizeXIndex]), float.Parse(lineContent[sizeYIndex]), 1);
                float platformBouncyPower = float.Parse(lineContent[bouncyPowerIndex]);
                
                platformGos[i-1] = MonoBehaviour.Instantiate(platformPrefab, platformPosition, Quaternion.identity, platformsParent.transform);
                platformGos[i-1].transform.localScale = platformScale;
                platformGos[i-1].GetComponent<BouncyPlatform>().JumpPower = platformBouncyPower;
            }

            ConvertToPrefabInstanceSettings setting = new ConvertToPrefabInstanceSettings();
            setting.recordPropertyOverridesOfMatches = true;
            setting.logInfo = false;
            setting.componentsNotMatchedBecomesOverride = true;
            setting.gameObjectsNotMatchedBecomesOverride = true;
            
            PrefabUtility.ConvertToPrefabInstances(platformGos, platformPrefab, setting, InteractionMode.UserAction);
        }
    }
}