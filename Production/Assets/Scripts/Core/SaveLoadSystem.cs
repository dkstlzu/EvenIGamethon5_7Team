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
        public bool LoadedData;
        public SaveData SaveData;
        public string DataSavingFolderPath = Path.Combine(Application.streamingAssetsPath, "Saves");
        private string DataSavingFileName = "Save";
        private string DataSavingExtension = ".txt";

        public string SaveDataFilePath => Path.Combine(DataSavingFolderPath, DataSavingFileName) + DataSavingExtension;

#if UNITY_EDITOR
        [MenuItem("Dev/CreateDefaultSaveData")]
        public static void CreateDefaultSaveDataFile()
        {
            SaveData data = new SaveData();
            string jsonData = JsonUtility.ToJson(data);
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
            
        public void Save(object data, string fileName)
        {
            string jsonData = JsonUtility.ToJson(data);
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
            Save(SaveData, DataSavingFileName);
        }

        public object Load(Type type, string path)
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

        public T Load<T>(string fileName)
        {
            return (T)Load(typeof(T), fileName);
        }

        public void LoadDatabase()
        {
            LoadedData = true;
            SaveData = Load<SaveData>(SaveDataFilePath);
        }
    }
}