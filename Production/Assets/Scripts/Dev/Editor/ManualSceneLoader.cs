﻿using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MoonBunny.Dev.Editor
{
    public class ManualSceneLoader
    {
        [MenuItem("Dev/Load Start Scene", priority = 2)]
        public static void StartScene()
        {
            SceneManager.LoadScene("1. Start");
        }
    }
}