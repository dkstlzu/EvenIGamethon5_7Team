using System;
using System.IO;
using System.Text;
using UnityEngine;
using Object = UnityEngine.Object;

namespace MoonBunny
{
    [Serializable]
    public class SaveLoadSystem : IDisposable
    {
        public SaveData SaveData;
        public string DataSavingFolderPath = Path.Combine(Application.streamingAssetsPath, "Saves");
        private string DataSavingFileName = "Save";
        private string DataSavingExtension = ".txt";

        public string SaveDataFilePath => Path.Combine(DataSavingFolderPath, DataSavingFileName) + DataSavingExtension;
        private FileStream _saveDataFileStream;
        
        public SaveLoadSystem()
        {
            _saveDataFileStream = File.OpenWrite(SaveDataFilePath);
        }
            
        public void Save(object data, string fileName)
        {
            string jsonData = JsonUtility.ToJson(data);
            byte[] byteData = Encoding.UTF8.GetBytes(jsonData);

            _saveDataFileStream.Write(byteData);
        }

        public void SaveDatabase()
        {
            Save(SaveData, DataSavingFileName);
        }

        public object Load(Type type, string path)
        {
            byte[] data = new byte[_saveDataFileStream.Length];
            string jsonData = String.Empty;
            int count;
            count = _saveDataFileStream.Read(data, 0, data.Length);
            if (count > 0)
            {
                jsonData = Encoding.UTF8.GetString(data, 0, count);
                return Convert.ChangeType(JsonUtility.FromJson(jsonData, type), type);
            }

            return null;
        }

        public T Load<T>(string fileName)
        {
            return (T)Load(typeof(T), fileName);
        }

        public void LoadDatabase()
        {
            SaveData = Load<SaveData>(SaveDataFilePath);
        }

        private string PathCombine(string fileName)
        {
            return Path.Combine(DataSavingFolderPath, fileName) + DataSavingExtension;
        }

        public void Dispose()
        {
            _saveDataFileStream?.Dispose();
        }
    }
}