using System;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MoonBunny.Dev.Editor
{
    [InitializeOnLoad]
    public static class CustomEditorPlayCallBack
    {
        public const string DefaultScenePath = "Assets/Scenes/" + SceneName.Loading + ".unity";
        public const string LastEditingScenePrefsKey = "LastEditingScenePath";

        static CustomEditorPlayCallBack()
        {
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
        }

        public static event Action OnExitingPlayMode;
        public static event Action OnEnteredPlayMode;
        public static event Action OnExtingEditMode;
        public static event Action OnEnteredEditMode;

        private static void OnPlayModeStateChanged(PlayModeStateChange state)
        {
            // Debug.Log($"Callback registerd on static ctor {state}");

            switch (state)
            {
                case PlayModeStateChange.EnteredEditMode:
                    OnEnteredEditMode?.Invoke();
                    break;
                case PlayModeStateChange.ExitingEditMode:
                    OnExtingEditMode?.Invoke();
                    break;
                case PlayModeStateChange.EnteredPlayMode:
                    OnEnteredPlayMode?.Invoke();
                    break;
                case PlayModeStateChange.ExitingPlayMode:
                    OnExitingPlayMode?.Invoke();
                    break;
            }
        }

        [MenuItem("Dev/PlayOnStartScnene %#p", priority = 0)]
        public static void PlayOnStartScene()
        {
            // Debug.Log("Static method");

            EditorPrefs.SetString(LastEditingScenePrefsKey, EditorSceneManager.GetActiveScene().path);

            OnEnteredEditMode += OnExitPlay;
            EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
            EditorSceneManager.OpenScene(DefaultScenePath);
            EditorApplication.isPlaying = true;
        }
        private static void OnExitPlay()
        {
            string path = EditorPrefs.GetString(LastEditingScenePrefsKey);

            // Debug.Log($"Callback registerd on static method onto ctor event : {path}");

            EditorSceneManager.OpenScene(path);
            OnEnteredEditMode -= OnExitPlay;
        }
    }
}