using System;
using System.IO;
using System.Security.Cryptography;
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
        public string PersistenceFolderPath => Path.Combine(Application.persistentDataPath, DataSavingFolderName);
        public string StreamingFolderPath => Path.Combine(Application.streamingAssetsPath, DataSavingFolderName);
        public string MapdataFolderPath => Path.Combine(Application.dataPath, DataSavingFolderName);
        private string DataSavingFolderName = "Saves";
        private string DataSavingFileName = "Save";
        private string DataSavingExtension = ".txt";

#if UNITY_EDITOR
        public string PersistenceFilePath => Path.Combine(PersistenceFolderPath, DataSavingFileName) + DataSavingExtension;
        public string StreamingFilePath => Path.Combine(StreamingFolderPath, DataSavingFileName) + DataSavingExtension;
        public string MapdataFilePath => Path.Combine(MapdataFolderPath, DataSavingFileName) + DataSavingExtension;
#else
        public string PersistenceFilePath => Path.Combine(PersistenceFolderPath, DataSavingFileName);
        public string StreamingFilePath => Path.Combine(StreamingFolderPath, DataSavingFileName);
        public string MapdataFilePath => Path.Combine(MapdataFolderPath, DataSavingFileName);
#endif


        private const string ENCRYPTION_KEY = "NUNETINE";
        private const string ENCRYPTION_IV = "NUNETINEIV";
        private static byte[] DesEncryptionKeyByte
        {
            get
            {
                byte[] bytes = Encoding.ASCII.GetBytes(ENCRYPTION_KEY);
                byte[] replacedBytes = new byte[8];
                
                FillOrClamp(bytes, replacedBytes);

                return replacedBytes;
            }
        }
        
        private static byte[] AesEncryptionKeyByte
        {
            get
            {
                byte[] bytes = Encoding.ASCII.GetBytes(ENCRYPTION_KEY);
                byte[] replacedBytes = new byte[32];
                
                FillOrClamp(bytes, replacedBytes);

                return replacedBytes;
            }
        }
        
        private static byte[] AesEncryptionIVByte
        {
            get
            {
                byte[] bytes = Encoding.ASCII.GetBytes(ENCRYPTION_IV);
                byte[] replacedBytes = new byte[16];
                
                FillOrClamp(bytes, replacedBytes);

                return replacedBytes;
            }
        }


        private static void FillOrClamp(byte[] source, byte[] destination)
        {
            if (source.Length <= 0)
            {
                FillOrClamp(Encoding.ASCII.GetBytes("DEFAULTK"), destination);
            } else if (source.Length < destination.Length)
            {
                int currentIndex = 0;

                while (currentIndex < destination.Length)
                {
                    int copylen = Mathf.Min(source.Length, destination.Length - currentIndex);
                    Array.Copy(source, 0, destination, currentIndex, copylen);
                    currentIndex += copylen;
                }
            } else if (source.Length > destination.Length)
            {
                Array.Copy(source, destination,  destination.Length);
            }
            else
            {
                Array.Copy(source, destination,  destination.Length);
            }
        }

        public SaveLoadSystem(string folder, string fileName) : this(folder, fileName, "txt")
        {

        }

        public SaveLoadSystem(string folder, string fileName, string extension)
        {
            DataSavingFolderName = folder;
            DataSavingFileName = fileName;
            DataSavingExtension = "." + extension;
        }
        
        void CheckDirectory()
        {
            string path = PersistenceFolderPath;

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
            string path = PersistenceFilePath;

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
#if UNITY_EDITOR
        [MenuItem("Dev/MapData/Encryption Test")]
        public static void Encrypt()
        {
            Encrypt(Path.Combine(Application.dataPath, "MapData", "Test.txt"), Path.Combine(Application.dataPath, "MapData", "TestEncrypt.txt"));
        }
        
        [MenuItem("Dev/MapData/Decryption Test")]
        public static void Decrypt()
        {
            Decrypt(Path.Combine(Application.dataPath, "MapData", "TestEncrypt.txt"), Path.Combine(Application.dataPath, "MapData", "Test.txt"));
        }
        
        [MenuItem("Dev/MapData/Self Encryption Test")]
        public static void SelfEncrypt()
        {
            Encrypt(Path.Combine(Application.dataPath, "MapData", "Test.txt"));
        }
        
        [MenuItem("Dev/Datas/Save Encrypted Test")]
        public static void SaveEncryptedTest()
        {
            GameManager.instance.SaveLoadSystem.SaveEncrypted(JsonUtility.ToJson(GameManager.instance.SaveLoadSystem.ProgressSaveData), (Path.Combine(Application.dataPath, "Mapdata", "Test")));
            AssetDatabase.Refresh();
        }

        [MenuItem("Dev/Datas/Load Encrypted Test")]
        public static void LoadEncryptedTest()
        {
            GameManager.instance.SaveLoadSystem.ProgressSaveData = GameManager.instance.SaveLoadSystem.LoadEncrypted<ProgressSaveData>(Path.Combine(Application.dataPath, "Mapdata", "Test"));
            AssetDatabase.Refresh();
        }
#endif

        public static void Encrypt(string fileName)
        {
            using (Aes aes = Aes.Create())
            {
                aes.Key = AesEncryptionKeyByte;
                aes.IV = AesEncryptionIVByte;

                byte[] data = File.ReadAllBytes(fileName);
                FileStream fs = new FileStream(fileName, FileMode.Create);
                Stream inStream = new MemoryStream(data);
            
                ICryptoTransform cryptoTransform = aes.CreateEncryptor();
                EncryptDecrypt(cryptoTransform, inStream, fs);

                fs.Close();
                inStream.Close();
            }
            
#if UNITY_EDITOR
            AssetDatabase.Refresh();
#endif
        }
        
        public static void Encrypt(string inputFileName, string outputFileName)
        {
            using (Aes aes = Aes.Create())
            {
                aes.Key = AesEncryptionKeyByte;
                aes.IV = AesEncryptionIVByte;

                FileStream infs = new FileStream(inputFileName, FileMode.Open);
                FileStream outfs = new FileStream(outputFileName, FileMode.Create);
            
                ICryptoTransform cryptoTransform = aes.CreateEncryptor();
                EncryptDecrypt(cryptoTransform, infs, outfs);

                outfs.Close();
                infs.Close();
            }
            
#if UNITY_EDITOR
            AssetDatabase.Refresh();
#endif
        }

        public static void Decrypt(string inputFileName, string outputFileName)
        {
            using (Aes aes = Aes.Create())
            {
                aes.Key = AesEncryptionKeyByte;
                aes.IV = AesEncryptionIVByte;

                FileStream infs = new FileStream(inputFileName, FileMode.Open);
                FileStream outfs = new FileStream(outputFileName, FileMode.Create);
            
                ICryptoTransform cryptoTransform = aes.CreateDecryptor();
                EncryptDecrypt(cryptoTransform, infs, outfs);
            
                outfs.Close();
                infs.Close();
            }
            
#if UNITY_EDITOR
            AssetDatabase.Refresh();
#endif
        }

        public static void EncryptDecrypt(ICryptoTransform cryptoTransform, Stream input, Stream output)
        {
            CryptoStream cryptoStream = new CryptoStream(output, cryptoTransform, CryptoStreamMode.Write);
            
            byte[] data = new byte[input.Length];

            input.Read(data);
            cryptoStream.Write(data);
            
            cryptoStream.Close();
        }
        
        public void SaveEncrypted(string str, string filePath)
        {
            using (Aes aes = Aes.Create())
            {
                aes.Key = AesEncryptionKeyByte;
                aes.IV = AesEncryptionIVByte;
            
                byte[] data = Encoding.Default.GetBytes(str);
                byte[] cryptoData = aes.CreateEncryptor().TransformFinalBlock(data, 0, data.Length);
                File.WriteAllBytes(filePath, cryptoData);
            }
        }
        
        public T LoadEncrypted<T>(string filePath)
        {
            using (Aes aes = Aes.Create())
            {
                aes.Key = AesEncryptionKeyByte;
                aes.IV = AesEncryptionIVByte;

                byte[] data = File.ReadAllBytes(filePath);
                byte[] cryptoData = aes.CreateDecryptor().TransformFinalBlock(data, 0, data.Length);
                string jsonData = Encoding.Default.GetString(cryptoData);

                return JsonUtility.FromJson<T>(jsonData);
            }
        }

        public void SaveString(string str)
        {
            CheckDirectory();
            CheckFile();
            
            using (FileStream fs = new FileStream(PersistenceFilePath, FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(fs))
                {
                    writer.Write(str);
                    MoonBunnyLog.print($"Save Data successfully on File {PersistenceFilePath}");
                    OnSaveSuccess?.Invoke();
                }
            }
        }
        
        public void SaveJson(object data)
        {
            if (data == null)
            {
                MoonBunnyLog.print("Cannot save data : SaveData is null");
                return;
            }
            
            
#if UNITY_EDITOR
            string jsonData = JsonUtility.ToJson(data, true);
            SaveString(jsonData);
#else
            string jsonData = JsonUtility.ToJson(data);
            SaveEncrypted(jsonData, PersistenceFilePath);
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

#if UNITY_EDITOR
            using (FileStream fs = File.OpenRead(PersistenceFilePath))
            {
                using (StreamReader reader = new StreamReader(fs))
                {
                    string jsonData = reader.ReadToEnd();
                    ProgressSaveData = JsonUtility.FromJson<ProgressSaveData>(jsonData);
                    if (ProgressSaveData != null)
                    {
                        MoonBunnyLog.print($"SaveLoadSystem load success : {PersistenceFilePath}");
                    }
                    else
                    {
                        LegacyProgressSaveData legacy = JsonUtility.FromJson<LegacyProgressSaveData>(jsonData);

                        if (legacy != null)
                        {
                            MoonBunnyLog.print($"SaveLoadSystem is legacy : {PersistenceFilePath}. So migrate to new form");
                            ProgressSaveData = ProgressSaveData.MigrateFromLegacy(legacy);
                        }
                        else
                        {
                            MoonBunnyLog.print($"SaveLoadSystem load fail : {PersistenceFilePath}.\n So made new one");
                            ProgressSaveData = ProgressSaveData.GetDefaultSaveData();
                        }
                    }
                }
            }
#else
            try
            {
                ProgressSaveData = LoadEncrypted<ProgressSaveData>(PersistenceFilePath);
            }
            catch (Exception)
            {
                MoonBunnyLog.print($"SaveLoadSystem is legacy before encrypting version {PersistenceFilePath}\n So made new encrypted one");
                ProgressSaveData = ProgressSaveData.GetDefaultSaveData();
            }
#endif

            DataIsLoaded = true;
            _onSaveDataLoaded?.Invoke();
        }

        public void LoadQuest()
        {
            CheckDirectory();
            CheckFile();

#if UNITY_EDITOR
            using (FileStream fs = File.OpenRead(PersistenceFilePath))
            {
                using (StreamReader reader = new StreamReader(fs))
                {
                    string jsonData = reader.ReadToEnd();
                    QuestSaveData = JsonUtility.FromJson<QuestSaveData>(jsonData);
                    if (QuestSaveData != null)
                    {
                        MoonBunnyLog.print($"SaveLoadSystem load success :{PersistenceFilePath}");
                    }
                    else
                    {
                        LegacyQuestSaveData legacy = JsonUtility.FromJson<LegacyQuestSaveData>(jsonData);

                        if (legacy != null)
                        {
                            MoonBunnyLog.print($"SaveLoadSystem is legacy : {PersistenceFilePath}. So migrate to new form");
                            QuestSaveData = QuestSaveData.MigrateFromLegacy(legacy);
                        }
                        else
                        {
                            MoonBunnyLog.print($"SaveLoadSystem load fail : {PersistenceFilePath}.\n So made new one");
                            QuestSaveData = QuestSaveData.GetDefaultSaveData();
                        }
                    }
                }
            }
#else
            try
            {
                QuestSaveData = LoadEncrypted<QuestSaveData>(PersistenceFilePath);            
            }
            catch (Exception)
            {
                MoonBunnyLog.print($"SaveLoadSystem is legacy before encrypting version {PersistenceFilePath}\n So made new encrypted one");
                QuestSaveData = QuestSaveData.GetDefaultSaveData();
            }
#endif
            
            DataIsLoaded = true;
            _onSaveDataLoaded?.Invoke();
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
            string directory = Path.Combine(Application.dataPath, "MapData");
            
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            string file = Path.Combine(Application.dataPath, "MapData", fileName + ".csv");
            
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
            File.AppendAllLines(PersistenceFilePath, fileStr);
            AssetDatabase.Refresh();
        }

        public void LoadCSV()
        {
            ConvertToPrefabInstanceSettings setting = new ConvertToPrefabInstanceSettings();
            setting.recordPropertyOverridesOfMatches = true;
            setting.logInfo = false;
            setting.componentsNotMatchedBecomesOverride = true;
            setting.gameObjectsNotMatchedBecomesOverride = true;
            
            Debug.Log("Load CSV From " + MapdataFilePath);
            string[] fileContents = File.ReadAllLines(MapdataFilePath);

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
                                        if (int.TryParse(str, out int targetIndex))
                                        {
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
                                        if (int.TryParse(str, out int targetIndex))
                                        {
                                            pattern2List.Add(targetIndex);
                                        }
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
                }
                catch (Exception e)
                {
                    Debug.LogError($"Error accured while loading csv data line : {i}\n{e}");
                }
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