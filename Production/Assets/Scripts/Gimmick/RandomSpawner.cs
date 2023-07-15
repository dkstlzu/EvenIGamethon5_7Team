using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace MoonBunny
{
    [DefaultExecutionOrder(-1)]
    public class RandomSpawner : Gimmick
    {
        public List<Vector2Int> PossiblePositionList;
        public GameObject FirstTarget;
        public GameObject SecondTarget;

#if UNITY_EDITOR
        public static Dictionary<int, RandomSpawner> SpawnerDict;

        public static void Init()
        {
            SpawnerDict = new Dictionary<int, RandomSpawner>();
        }

        public static void Uninit()
        {
            SpawnerDict.Clear();
            SpawnerDict = null;
        }
        
        public static GameObject MakeNew(GameObject firstTargetPrefab, GameObject secondTargetPrefab, Vector2Int position, int id)
        {
            ConvertToPrefabInstanceSettings setting = new ConvertToPrefabInstanceSettings();
            setting.recordPropertyOverridesOfMatches = true;
            setting.logInfo = false;
            setting.componentsNotMatchedBecomesOverride = true;
            setting.gameObjectsNotMatchedBecomesOverride = true;
            
            string Path = "Assets/Prefabs/Level/RandomSpawner.prefab";

            RandomSpawner spawner;
            if (!SpawnerDict.TryGetValue(id, out spawner))
            {
                GameObject PrefabAsset = AssetDatabase.LoadAssetAtPath<GameObject>(Path);
                spawner = Instantiate(PrefabAsset, GameObject.FindWithTag("Level").transform).GetComponent<RandomSpawner>();
                spawner.SetTargets(firstTargetPrefab, secondTargetPrefab);
                PrefabUtility.ConvertToPrefabInstance(spawner.gameObject, PrefabAsset, setting, InteractionMode.UserAction);
                SpawnerDict.Add(id, spawner);
            }
            
            spawner.PossiblePositionList.Add(position);
            
            return spawner.gameObject;
        }
#endif
        
        public void SetTargets(GameObject firstTargetPrefab, GameObject secondTargetPrefab)
        {
            FirstTarget = firstTargetPrefab;
            SecondTarget = secondTargetPrefab;
        }

        private void Start()
        {
#if UNITY_EDITOR
            if (!EditorApplication.isPlaying) return;
#endif
            Transform parent = GameObject.FindWithTag("Obstacles").transform;

            GameObject targetPrefab = (Random.value > 0.5) ? FirstTarget : SecondTarget;

            int count = PossiblePositionList.Count;

            Vector2Int position = PossiblePositionList[Random.Range(0, count)];

            Instantiate(targetPrefab, GridTransform.ToReal(position), Quaternion.identity, parent);
            
            Destroy(gameObject);
        }
    }
}