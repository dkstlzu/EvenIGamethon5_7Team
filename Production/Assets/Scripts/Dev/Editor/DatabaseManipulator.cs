using System;
using System.IO;
using System.Text;
using NUnit.Framework.Constraints;
using UnityEditor;
using UnityEngine;

namespace MoonBunny.Dev.Editor
{
    public static class DatabaseManipulator
    {
        private const string CollectionAssetPath = "Assets/Resources/Specs/FriendCollection.asset"; 
        private const string DefaultCollectionAssetPath = "Assets/Resources/Specs/DefaultFriendCollection.asset";

        [MenuItem("Dev/ResetDatas", priority = 10)]
        static void ResetDatas()
        {
            ResetCollection();
            ResetFirstPlay();
        }

        private static void ResetFirstPlay()
        {
            PlayerPrefs.SetInt("MoonBunnyFirstPlay", 1);
        }

        static void ResetCollection()
        {
            FriendCollection collection = AssetDatabase.LoadAssetAtPath<FriendCollection>(CollectionAssetPath);
            FriendCollection defaultCollection = AssetDatabase.LoadAssetAtPath<FriendCollection>(DefaultCollectionAssetPath);

            Debug.Log($"DataBase Reset {collection} to {defaultCollection}");
            
            collection.Datas.Clear();
            for (int i = 0; i < defaultCollection.Datas.Count; i++)
            {
                FriendCollection.Data data = new FriendCollection.Data();
                data.Name = defaultCollection.Datas[i].Name;
                data.TargetCollectingNumber = defaultCollection.Datas[i].TargetCollectingNumber;
                data.CurrentCollectingNumber = defaultCollection.Datas[i].CurrentCollectingNumber;
                
                collection.Datas.Add(data);
            }
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
    }
}