using System;
using System.IO;
using System.Linq;
using System.Text;
using NUnit.Framework.Constraints;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MoonBunny.Dev.Editor
{
    public static class DatabaseManipulator
    {
        private const string CollectionAssetPath = "Assets/Resources/Specs/FriendCollection.asset"; 
        private const string DefaultCollectionAssetPath = "Assets/Resources/Specs/DefaultFriendCollection.asset";

        [MenuItem("Dev/Datas/ResetDatas", priority = 10)]
        static void ResetDatas()
        {
            ResetCollection();
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
        
        
        [MenuItem("Dev/Datas/CreateDefaultSaveData")]
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
        
        [MenuItem("Dev/Datas/CreateFullSaveData")]
        public static void CreateFullSaveDataFile()
        {
            SaveData data = new SaveData();
            
            FriendCollection collection = AssetDatabase.LoadAssetAtPath<FriendCollection>(CollectionAssetPath);

            for (int i = 0; i < data.CollectionDict.Count; i++)
            {
                data.CollectionDict[(FriendName)i] = collection.Datas[i].TargetCollectingNumber;
            }

            for (int i = 0; i < data.ClearDict.Count; i++)
            {
                data.ClearDict[(StageName)i] = 3;
            }

            data.ShowTutorial = false;
            data.GoldNumber = 9999;

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

        [MenuItem("Dev/MapData/HorizontalVerticalReverse")]
        public static void ReverseBouncyPlatformHorizontalVerticalMoveRange()
        {
            if (!SceneName.StageNames.Contains(SceneManager.GetActiveScene().name))
            {
                EditorUtility.DisplayDialog("Wrong Scene", "스테이지 씬에서만 이 기능을 사용하세요", "확인");
                return;
            }

            BouncyPlatform[] bouncyPlatforms = GameObject.FindObjectsByType<BouncyPlatform>(FindObjectsSortMode.None);

            foreach (var bouncyPlatform in bouncyPlatforms)
            {
                (bouncyPlatform.HorizontalMoveRange, bouncyPlatform.VerticalMoveRange) = (bouncyPlatform.VerticalMoveRange, bouncyPlatform.HorizontalMoveRange);
            }

            EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
        }
    }
}