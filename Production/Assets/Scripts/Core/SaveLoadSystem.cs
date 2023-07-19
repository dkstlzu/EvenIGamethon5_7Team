using System;
using System.IO;
using System.Text;
using MoonBunny.Dev;
using UnityEngine;
using UnityEngine.Networking;

#if UNITY_EDITOR
using System.Globalization;
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

            AssetDatabase.Refresh();
        }
#endif
            
        public void SaveJson(object data)
        {
            if (data == null)
            {
                MoonBunnyLog.print("Cannot save data : SaveData is null");
                return;
            }
            
            string jsonData = JsonUtility.ToJson(data, true);
            byte[] byteData = Encoding.UTF8.GetBytes(jsonData);

// #if UNITY_ANDROID
            // UnityWebRequest request = UnityWebRequest.PostWwwForm(SaveDataFilePath, jsonData);
            // request.SendWebRequest().completed += (ao) =>
            // {
            //     MoonBunnyLog.print("Save Data successfully : is Android");
            // };
            //
// #else
            File.WriteAllText(SaveDataFilePath, String.Empty);

            using (FileStream fs = File.OpenWrite(SaveDataFilePath))
            {
                fs.Write(byteData);
                fs.Flush();
                MoonBunnyLog.print("Save Data successfully");
            }
// #endif


#if UNITY_EDITOR
            AssetDatabase.Refresh();
#endif
        }

        public void SaveDatabase()
        {
            SaveJson(SaveData);
        }

        public object LoadJson(Type type)
        {
#if UNITY_ANDROID
            MoonBunnyLog.print("SaveLoadSystem load : is Android");

            UnityWebRequest request = UnityWebRequest.Get(SaveDataFilePath);
            var oper = request.SendWebRequest();
            // oper.completed += (operation) =>
            // {
            //     string jsonData;
            //     jsonData = Encoding.UTF8.GetString(oper.webRequest.downloadHandler.data);
            //     return Convert.ChangeType(JsonUtility.FromJson(jsonData, type), type);
            // };

            while (!request.isDone)
            {
                if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
                {
                    break;
                }
            }

            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                
            }
            else
            {
                string jsonData;
                jsonData = Encoding.UTF8.GetString(oper.webRequest.downloadHandler.data);
                return Convert.ChangeType(JsonUtility.FromJson(jsonData, type), type);
            }
#else
            MoonBunnyLog.print("SaveLoadSystem load : is not Android");

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
#endif
            return null;
        }

        public T LoadJson<T>()
        {
            return (T)LoadJson(typeof(T));
        }

        public void LoadDatabase()
        {
            SaveData = LoadJson<SaveData>();
            if (SaveData != null)
            {
                MoonBunnyLog.print("SaveData is loaded successfully");
                DataIsLoaded = true;
            }
            else
            {
                MoonBunnyLog.print("SaveData load failed");
            }
        }

#if UNITY_EDITOR
        private static string CSVSeperator = ",";

        private static string[] CSVHeader = new string[]
        {
            "No",
            "Type",
            "X",
            "Y",
            "Value1",
            "Value2",
            "Value3",
            "Value4",
            "Value5",
            "Value6",
        };

        public static void CreateDefaultMapData(string fileName)
        {
            string directory = Path.Combine(Application.streamingAssetsPath, "MapData");
            
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            string file = Path.Combine(Application.streamingAssetsPath, "MapData", fileName + ".csv");
            
            using (var writer = File.CreateText(file))
            {
                StringBuilder str = new StringBuilder();
                str.AppendJoin(CSVSeperator, CSVHeader);
                writer.WriteLine(str);
            }
            
            AssetDatabase.Refresh();
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

            CreateDefaultMapData(DataSavingFileName);
            File.AppendAllLines(SaveDataFilePath, fileStr);
            AssetDatabase.Refresh();
        }

        public void LoadCSV()
        {
            ConvertToPrefabInstanceSettings setting = new ConvertToPrefabInstanceSettings();
            setting.recordPropertyOverridesOfMatches = true;
            setting.logInfo = false;
            setting.componentsNotMatchedBecomesOverride = true;
            setting.gameObjectsNotMatchedBecomesOverride = true;

            // Gimmick[] gimmicksInSceneAlready = GameObject.FindObjectsByType<Gimmick>(FindObjectsInactive.Include, FindObjectsSortMode.None);
            //
            // foreach (Gimmick platform in gimmicksInSceneAlready)
            // {
            //     MonoBehaviour.DestroyImmediate(platform.gameObject);
            // }
            
            string[] fileContents = File.ReadAllLines(SaveDataFilePath);

            int typeIndex = Array.IndexOf(CSVHeader, "Type");
            int xIndex = Array.IndexOf(CSVHeader, "X");
            int yIndex = Array.IndexOf(CSVHeader, "Y");
            int bouncyPlatformPowerIndex = Array.IndexOf(CSVHeader, "Value1");
            int bouncyPlatformHorizontalMoveIndex = Array.IndexOf(CSVHeader, "Value2");
            int bouncyPlatformVerticalMoveIndex = Array.IndexOf(CSVHeader, "Value3");
            int randomSpawnerType1Index = Array.IndexOf(CSVHeader, "Value1");
            int randomSpawnerValue1Index = Array.IndexOf(CSVHeader, "Value2");
            int randomSpawnerType2Index = Array.IndexOf(CSVHeader, "Value3");
            int randomSpawnerValue2Index = Array.IndexOf(CSVHeader, "Value4");
            int randomSpawnerIDIndex = Array.IndexOf(CSVHeader, "Value5");

            NumberFormatInfo numberFormatInfo = new NumberFormatInfo();
            numberFormatInfo.NegativeSign = "-";
            
            RandomSpawner.Init();
            
            Debug.Log($"Load CSV Data total line : {fileContents.Length}");

            for (int i = 1; i < fileContents.Length; i++)
            {
                try
                {
                    string[] lineContent = fileContents[i].Split(CSVSeperator);

                    if (lineContent.Length == 0)
                    {
                        break;
                    }

                    Vector2Int gridPosition = new Vector2Int(int.Parse(lineContent[xIndex], numberFormatInfo),
                        int.Parse(lineContent[yIndex], numberFormatInfo));
                    
                    Vector3 realPosition = GridTransform.ToReal(gridPosition);
                    
                    Transform objectParent = GameObject.FindWithTag(GetTargetTag(lineContent[typeIndex])).transform;

                    GameObject targetPrefab = GetTargetPrefab(lineContent[typeIndex]);

                    if (targetPrefab == null)
                    {
                        Debug.LogError($"There is no prefab of {lineContent[typeIndex]} check again");
                        continue;
                    }

                    GameObject instantiatedGo = null;

                    if (lineContent[typeIndex] == "RandomSpawner")
                    {
                        instantiatedGo = RandomSpawner.MakeNew(GetTargetPrefab(lineContent[randomSpawnerType1Index]),
                            GetTargetPrefab(lineContent[randomSpawnerType2Index]),
                            gridPosition,
                            int.Parse(lineContent[randomSpawnerIDIndex]));
                    } else if (lineContent[typeIndex] == "StageGoal")
                    {
                        GameObject.FindWithTag("StageGoal").transform.position = realPosition;
                        continue;
                    } else if (lineContent[typeIndex] == "SpawnPoint")
                    {
                        GameObject.FindWithTag("DefaultStartPosition").transform.position = realPosition;
                        continue;
                    }
                    else
                    {
                        instantiatedGo = MonoBehaviour.Instantiate(targetPrefab, realPosition, Quaternion.identity, objectParent);
                    }

                    if (instantiatedGo == null)
                    {
                        Debug.LogError($"Something wrong while instantiating {lineContent[typeIndex]} check again");
                        continue;
                    }

                    switch (lineContent[typeIndex])
                    {
                        case "BouncyPlatform":
                            BouncyPlatform platform = instantiatedGo.GetComponent<BouncyPlatform>();
                            platform.JumpPower = int.Parse(lineContent[bouncyPlatformPowerIndex]);
                            platform.VerticalMoveRange = int.Parse(lineContent[bouncyPlatformVerticalMoveIndex]);
                            platform.HorizontalMoveRange = int.Parse(lineContent[bouncyPlatformHorizontalMoveIndex]);
                            break;
                    }

                    if (PrefabUtility.GetPrefabAssetType(instantiatedGo) == PrefabAssetType.NotAPrefab)
                    {
                        PrefabUtility.ConvertToPrefabInstance(instantiatedGo, targetPrefab, setting, InteractionMode.UserAction);
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError(e + $"\nError accured while loading csv data line : {i}");
                }
            }

            RandomSpawner.Uninit();
        }

        private string GetTypeInCSV(GridObject obj)
        {
            string result = string.Empty;
            
            switch (obj)
            {
                case BouncyPlatform platform:
                    break;
                case CrackPlatform platform:
                    break;
                case Rocket rocket:
                    break;
                case Magnet magnet:
                    break;
                case RandomSpawner spawner:
                    break;
                case ItemBox itemBox:
                    break;
                case ShootingStar shootingStar:
                    break;
            }

            return result;
        }

        private Type GetTypeFromCSV(string typeStr)
        {
            Type type = null;
            
            switch (typeStr)
            {
                
            }

            return type;
        }
        
        private string GetTargetTag(string type)
        {
            string result = "Level";
            
            switch (type)
            {
                case "BouncyPlatform":
                case "CrackPlatform":
                    result = "Platforms";
                    break;
                case "Rocket":
                case "Magnet":
                case "ItemBox":
                    result = "Items";        
                    break;
                case "ShootingStar":
                    result = "Obstacles";    
                    break;
            }

            return result;
        }

        private const string GimmickPath = "Assets/Prefabs/Level/";
        
        private string GetTargetPath(string type)
        {
            string result = "";
            
            switch (type)
            {
                case "Rocket":
                case "Magnet":
                case "Item":
                case "Heart":
                    result = Path.Combine(GimmickPath, "Items", type + ".prefab");
                    break;
                case "Bee":
                case "CarnivorousFlower":
                case "Crow":
                case "Obstacle":
                case "PinWheel":
                case "ShootingStar":
                case "SpiderWeb":
                    result = Path.Combine(GimmickPath, "Obstacles", type + ".prefab");
                    break;
                default:
                    result = Path.Combine(GimmickPath, type + ".prefab");
                    break;
            }
            
            return result;
        }

        private GameObject GetTargetPrefab(string type)
        {
            return AssetDatabase.LoadAssetAtPath<GameObject>(GetTargetPath(type));
        }
#endif
    }
}