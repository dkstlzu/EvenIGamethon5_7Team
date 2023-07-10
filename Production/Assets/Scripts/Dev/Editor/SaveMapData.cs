using UnityEditor;

namespace MoonBunny.Dev.Editor
{
    public class SaveMapData
    {
        [MenuItem("Dev/SaveMapData")]
        public static void SaveData()
        {
            SaveLoadSystem system = new SaveLoadSystem("MapData", "Map", "csv");
            
            system.SaveCSV();
        }
        
        [MenuItem("Dev/LoadMapData")]
        public static void LoadData()
        {
            SaveLoadSystem system = new SaveLoadSystem("MapData", "Map", "csv");
            
            system.LoadCSV();
        }
    }
}