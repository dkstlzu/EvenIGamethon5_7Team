﻿using System;
using System.IO;
using System.Text;
using MoonBunny.Dev;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Serialization;

#if UNITY_EDITOR
using System.Collections.Generic;
using System.Globalization;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
#endif

namespace MoonBunny
{
    [Serializable]
    public class SaveLoadSystem
    {
        public bool DataIsLoaded;
        public ProgressSaveData ProgressSaveData;
        public QuestSaveData QuestSaveData;
        public string DataSavingFolderPath => Path.Combine(Application.persistentDataPath, DataSavingFolderName);
        private string DataSavingFolderName = "Saves";
        private string DataSavingFileName = "Save";
        private string DataSavingExtension = ".txt";

        public string SaveDataFilePath => Path.Combine(DataSavingFolderPath, DataSavingFileName) + DataSavingExtension;

        public SaveLoadSystem()
        {
        }

        public SaveLoadSystem(string folder, string fileName, string extension)
        {
            DataSavingFolderName = folder;
            DataSavingFileName = fileName;
            DataSavingExtension = "." + extension;
        }

        public void Init(string folder, string fileName, string extension)
        {
            DataSavingFolderName = folder;
            DataSavingFileName = fileName;
            DataSavingExtension = "." + extension;
        }
        
        void CheckDirectory()
        {
            string path = DataSavingFolderPath;

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            while (!Directory.Exists(path))
            {
                
            }
        }

        void CheckFile()
        {
            string path = SaveDataFilePath;

            if (!File.Exists(path))
            {
                using (File.Create(path))
                {
                    
                }
            }

            while (!File.Exists(path))
            {
                
            }
        }

        public event Action OnSaveSuccess;
        
        public void SaveJson(object data)
        {
            if (data == null)
            {
                MoonBunnyLog.print("Cannot save data : SaveData is null");
                return;
            }
            
            CheckDirectory();
            CheckFile();
            
            string jsonData = JsonUtility.ToJson(data, true);
// #if UNITY_EDITOR
            using (FileStream fs = new FileStream(SaveDataFilePath, FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(fs))
                {
                    writer.Write(jsonData);
                    MoonBunnyLog.print($"Save Data successfully on File {SaveDataFilePath}");
                    OnSaveSuccess?.Invoke();
                }
            }
// #else
             
            // UnityWebRequest uwr = UnityWebRequest.PostWwwForm(SaveDataFilePath, jsonData);
            // uwr.SendWebRequest().completed += (ao) =>
            // {
            //     MoonBunnyLog.print("Save Data successfully on UnityWebRequest");
            //     OnSaveSuccess?.Invoke();
            // };
// #endif

#if UNITY_EDITOR
            AssetDatabase.Refresh();
#endif
        }

        public void SaveProgress()
        {
            SaveJson(ProgressSaveData);
        }

        public void SaveQuest()
        {
            SaveJson(QuestSaveData);
        }

        public object LoadJson(Type type)
        {
// #if UNITY_ANDROID
            // MoonBunnyLog.print("SaveLoadSystem load : is Android");
            //
            // UnityWebRequest request = UnityWebRequest.Get(SaveDataFilePath);
            // var oper = request.SendWebRequest();
            //
            // while (!request.isDone)
            // {
            //     if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            //     {
            //         break;
            //     }
            // }
            //
            // if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            // {
            //     
            // }
            // else
            // {
            //     string jsonData = Encoding.UTF8.GetString(oper.webRequest.downloadHandler.data);
            //     return Convert.ChangeType(JsonUtility.FromJson(jsonData, type), type);
            // }
// #else

            using (FileStream fs = File.OpenRead(SaveDataFilePath))
            {
                using (StreamReader reader = new StreamReader(fs))
                {
                    string jsonData = reader.ReadToEnd();
                    DataIsLoaded = true;
                    MoonBunnyLog.print($"SaveLoadSystem load success :{SaveDataFilePath}");
                    return Convert.ChangeType(JsonUtility.FromJson(jsonData, type), type);
                }
            }
// #endif
            return null;
        }

        public T LoadJson<T>()
        {
            return (T)LoadJson(typeof(T));
        }

        private event Action _onSaveDataLoaded;
        public event Action OnSaveDataLoaded
        {
            add
            {
                if (DataIsLoaded) value?.Invoke();
                else _onSaveDataLoaded += value;
            }
            remove
            {
                _onSaveDataLoaded -= value;
            }
        }

        public void LoadProgress()
        {
            CheckDirectory();
            CheckFile();

            ProgressSaveData = LoadJson<ProgressSaveData>();
            _onSaveDataLoaded?.Invoke();
            
            // UnityWebRequest request = UnityWebRequest.Get(SaveDataFilePath);
            // request.SendWebRequest().completed += (ao) =>
            // {
            //     var uwr = ((UnityWebRequestAsyncOperation)ao).webRequest;
            //
            //     try
            //     {
            //         string jsonData = Encoding.UTF8.GetString(uwr.downloadHandler.data);
            //         ProgressSaveData = (ProgressSaveData)JsonUtility.FromJson(jsonData, typeof(ProgressSaveData));
            //         
            //         MoonBunnyLog.print("SaveData is loaded successfully");
            //     }
            //     catch (Exception)
            //     {
            //         ProgressSaveData = ProgressSaveData.GetDefaultSaveData();
            //         SaveProgress();
            //
            //         MoonBunnyLog.print("SaveData is loaded fail so made new one");
            //     }
            //     finally
            //     {
            //         DataIsLoaded = true;
            //         _onSaveDataLoaded?.Invoke();
            //     }
            // };
        }

        public void LoadQuest()
        {
            CheckDirectory();
            CheckFile();

            QuestSaveData = LoadJson<QuestSaveData>();
            _onSaveDataLoaded?.Invoke();

            // UnityWebRequest request = UnityWebRequest.Get(SaveDataFilePath);
            // request.SendWebRequest().completed += (ao) =>
            // {
            //     var uwr = ((UnityWebRequestAsyncOperation)ao).webRequest;
            //
            //     try
            //     {
            //         string jsonData = Encoding.UTF8.GetString(uwr.downloadHandler.data);
            //         QuestSaveData = (QuestSaveData)JsonUtility.FromJson(jsonData, typeof(QuestSaveData));
            //         
            //         MoonBunnyLog.print("QuestData is loaded successfully");
            //     }
            //     catch (Exception)
            //     {
            //         QuestSaveData = QuestSaveData.GetDefaultSaveData();
            //         SaveQuest();
            //         
            //         MoonBunnyLog.print("QuestData is loaded fail so made new one");
            //     }
            //     finally
            //     {
            //         DataIsLoaded = true;
            //         _onSaveDataLoaded?.Invoke();
            //
            //     }
            // };
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

        private static string NoData = "-";

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
            
            Debug.Log("DB" + SaveDataFilePath);
            string[] fileContents = File.ReadAllLines(SaveDataFilePath);

            int noIndex = Array.IndexOf(CSVHeader, "No");
            int typeIndex = Array.IndexOf(CSVHeader, "Type");
            int xIndex = Array.IndexOf(CSVHeader, "X");
            int yIndex = Array.IndexOf(CSVHeader, "Y");
            int bouncyPlatformPowerIndex = Array.IndexOf(CSVHeader, "Value1");
            int bouncyPlatformVerticalMoveIndex = Array.IndexOf(CSVHeader, "Value2");
            int bouncyPlatformHorizontalMoveIndex = Array.IndexOf(CSVHeader, "Value3");
            int bouncyPlatformSpeedIndex = Array.IndexOf(CSVHeader, "Value4");
            int bouncyPlatformPattern1Index = Array.IndexOf(CSVHeader, "Value5");
            int bouncyPlatformPattern2Index = Array.IndexOf(CSVHeader, "Value6");
            int randomSpawnerType1Index = Array.IndexOf(CSVHeader, "Value1");
            int randomSpawnerValue1Index = Array.IndexOf(CSVHeader, "Value2");
            int randomSpawnerType2Index = Array.IndexOf(CSVHeader, "Value3");
            int randomSpawnerValue2Index = Array.IndexOf(CSVHeader, "Value4");
            int randomSpawnerIDIndex = Array.IndexOf(CSVHeader, "Value5");

            NumberFormatInfo numberFormatInfo = new NumberFormatInfo();
            numberFormatInfo.NegativeSign = "-";
            
            RandomSpawner.Init();
            
            Debug.Log($"Load CSV Data total line : {fileContents.Length}");

            Dictionary<int, BouncyPlatform> bouncyPlatformDict = new Dictionary<int, BouncyPlatform>();
            Dictionary<int, List<int>> bouncyPlatformPattern1Dict = new Dictionary<int, List<int>>();
            Dictionary<int, List<int>> bouncyPlatformPattern2Dict = new Dictionary<int, List<int>>();

            for (int i = 1; i < fileContents.Length; i++)
            {
                // try
                // {
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

                    if (lineContent[typeIndex] == "GoalInPlatform" || lineContent[typeIndex] == "SpawnPoint")
                    {
                        
                    } else if (targetPrefab == null)
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
                    } else if (lineContent[typeIndex] == "GoalInPlatform")
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

                            string verticalRangeStr = lineContent[bouncyPlatformVerticalMoveIndex];

                            if (verticalRangeStr != NoData)
                            {
                                platform.VerticalMoveRange = int.Parse(verticalRangeStr);
                            }

                            string horizontalRangeStr = lineContent[bouncyPlatformHorizontalMoveIndex];

                            if (horizontalRangeStr != NoData)
                            {
                                platform.HorizontalMoveRange = int.Parse(horizontalRangeStr);
                            }

                            string speedStr = lineContent[bouncyPlatformSpeedIndex];

                            if (speedStr != NoData)
                            {
                                platform.LoopCycleSpeed = float.Parse(speedStr);
                            }

                            int index = int.Parse(lineContent[noIndex]);
                            platform.Index = index;
                            bouncyPlatformDict.Add(index, platform);

                            string[] pattern1Str = lineContent[bouncyPlatformPattern1Index].Split(":");
                            string[] pattern2Str = lineContent[bouncyPlatformPattern2Index].Split(":");

                            if (pattern1Str.Length > 0)
                            {
                                List<int> pattern1List = new List<int>();
                                
                                foreach (var str in pattern1Str)
                                {
                                    if (str != NoData)
                                    {
                                        int targetIndex = int.Parse(str);
                                        if (targetIndex > 0)
                                        {
                                            pattern1List.Add(targetIndex);
                                        }
                                        else
                                        {
                                            platform.MakeVirtual();
                                        }
                                    }
                                }
                                
                                bouncyPlatformPattern1Dict.Add(index, pattern1List);
                            }
                            
                            if (pattern2Str.Length > 0)
                            {
                                List<int> pattern2List = new List<int>();
                                
                                foreach (var str in pattern2Str)
                                {
                                    if (str != NoData)
                                    {
                                        pattern2List.Add(int.Parse(str));
                                    }
                                }
                                
                                bouncyPlatformPattern2Dict.Add(index, pattern2List);
                            }
                            

                            break;
                    }

                    if (PrefabUtility.GetPrefabAssetType(instantiatedGo) == PrefabAssetType.NotAPrefab)
                    {
                        PrefabUtility.ConvertToPrefabInstance(instantiatedGo, targetPrefab, setting, InteractionMode.UserAction);
                    }
                // }
                // catch (Exception e)
                // {
                //     Debug.LogError(e + $"\nError accured while loading csv data line : {i}");
                // }
            }

            foreach (var pair in bouncyPlatformPattern1Dict)
            {
                BouncyPlatform platform = bouncyPlatformDict[pair.Key];

                foreach (int index in pair.Value)
                {
                    platform.Pattern1PlatformList.Add(bouncyPlatformDict[index]);
                }
            }
            
            foreach (var pair in bouncyPlatformPattern2Dict)
            {
                BouncyPlatform platform = bouncyPlatformDict[pair.Key];

                foreach (int index in pair.Value)
                {
                    platform.Pattern2PlatformList.Add(bouncyPlatformDict[index]);
                }
            }

            RandomSpawner.Uninit();
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
                case "Heart":
                    result = "Items";        
                    break;
                case "ShootingStar":
                case "Bee":
                case "Crow":
                case "Cannibalism":
                case "Thunderbolt":
                case "SpiderWeb":
                case "PinWheel":
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
                case "Cannibalism":
                case "Crow":
                case "Obstacle":
                case "PinWheel":
                case "ShootingStar":
                case "SpiderWeb":
                case "Thunderbolt":
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