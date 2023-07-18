using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace dkstlzu.Utility
{
    public class SceneLoadCallbackSetter
    {
        public List<string> SceneNameList = new List<string>();

        public SceneLoadCallbackSetter(IEnumerable<string> sceneNames)
        {
            SceneNameList.AddRange(sceneNames);

            foreach(string sceneName in SceneNameList)
            {
                SSceneLoadCallBackDict.Add(sceneName, delegate{});
                SSceneUnloadCallBackDict.Add(sceneName, delegate{});
            }
        }

        public static Dictionary<string, Action> SSceneLoadCallBackDict = new Dictionary<string, Action>();
        public static Dictionary<string, Action> SSceneUnloadCallBackDict = new Dictionary<string, Action>();
        public Dictionary<string, Action> SceneLoadCallBackDict => SSceneLoadCallBackDict;
        public Dictionary<string, Action> SceneUnloadCallBackDict => SSceneUnloadCallBackDict;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Init()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            SceneManager.sceneUnloaded -= OnSceneUnloaded;
            
            SSceneLoadCallBackDict.Clear();
            SSceneUnloadCallBackDict.Clear();
            
            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.sceneUnloaded += OnSceneUnloaded;
        }

        [InitializeOnLoadMethod]
        static void Uninit()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            SceneManager.sceneUnloaded -= OnSceneUnloaded;
            
            SSceneLoadCallBackDict.Clear();
            SSceneUnloadCallBackDict.Clear();
        }

        static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            Action action;
            if (!SSceneLoadCallBackDict.TryGetValue(scene.name, out action))
            {
                Debug.Log($"Scene CallBack do not have Scene named {scene.name}");
                return;
            }

            action?.Invoke();
        }

        static void OnSceneUnloaded(Scene scene)
        {
            Action action;
            if (!SSceneUnloadCallBackDict.TryGetValue(scene.name, out action))
            {
                Debug.Log($"Scene CallBack do not have Scene named {scene.name}");
                return;
            }

            action?.Invoke();            
        }
    }
}