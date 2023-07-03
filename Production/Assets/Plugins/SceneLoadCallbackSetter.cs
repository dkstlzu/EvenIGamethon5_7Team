using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace dkstlzu.Utility
{
    public class SceneLoadCallbackSetter : IDisposable
    {
        public static bool _initiated = false;
        public List<string> SceneNameList = new List<string>();

        public SceneLoadCallbackSetter(IEnumerable<string> sceneNames)
        {
            SceneNameList.AddRange(sceneNames);

            foreach(string sceneName in SceneNameList)
            {
                SSceneLoadCallBackDict.Add(sceneName, delegate{});
                SSceneUnloadCallBackDict.Add(sceneName, delegate{});
            }

            Init();
            
#if UNITY_EDITOR
            AssemblyReloadEvents.beforeAssemblyReload += Dispose;
#endif
        }

        [RuntimeInitializeOnLoadMethod]
        ~SceneLoadCallbackSetter()
        {
            Dispose();
        }

        public static Dictionary<string, Action> SSceneLoadCallBackDict = new Dictionary<string, Action>();
        public static Dictionary<string, Action> SSceneUnloadCallBackDict = new Dictionary<string, Action>();
        public Dictionary<string, Action> SceneLoadCallBackDict => SSceneLoadCallBackDict;
        public Dictionary<string, Action> SceneUnloadCallBackDict => SSceneUnloadCallBackDict;

        private static void Init()
        {
            if (_initiated) return;

            _initiated = true;

            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.sceneUnloaded += OnSceneUnloaded;
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
                Debug.Log($"Scene CallBack do not have Scene named{scene.name}");
                return;
            }

            action?.Invoke();            
        }

        public void Dispose()
        {
            _initiated = false;
                        
            SceneManager.sceneLoaded -= OnSceneLoaded;
            SceneManager.sceneUnloaded -= OnSceneUnloaded;
            
            SSceneLoadCallBackDict.Clear();
            SSceneUnloadCallBackDict.Clear();
            
#if UNITY_EDITOR
            AssemblyReloadEvents.beforeAssemblyReload -= Dispose;
#endif
        }
    }
}