﻿using System;
using dkstlzu.Utility;
using MoonBunny.UIs;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MoonBunny
{
    public class InputManager : Singleton<InputManager>
    {
        public MoonBunnyInputAsset InputAsset;

        private void Awake()
        {
            InputAsset = new MoonBunnyInputAsset();
            InputAsset.Enable();

            DisableIngameInput();

            GameManager.instance.OnStageSceneLoaded += () => EnableIngameInput();
            GameManager.instance.OnStageSceneUnloaded += () => DisableIngameInput();
        }

        public void EnableIngameInput()
        {
            InputAsset.Ingame.Enable();
        }

        public void DisableIngameInput()
        {
            InputAsset.Ingame.Disable();
        }
    }
}