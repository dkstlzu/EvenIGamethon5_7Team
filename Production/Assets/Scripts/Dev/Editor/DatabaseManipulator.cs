using NUnit.Framework.Constraints;
using UnityEditor;
using UnityEngine;

namespace MoonBunny.Dev.Editor
{
    public static class DatabaseManipulator
    {
        private const string CollectionAssetPath = "Assets/Resources/Specs/FriendCollection.asset"; 
        private const string DefaultCollectionAssetPath = "Assets/Resources/Specs/DefaultFriendCollection.asset";

        [MenuItem("Dev/ResetDatas")]
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
    }
}