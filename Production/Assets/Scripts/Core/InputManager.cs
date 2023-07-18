using System;
using MoonBunny.UIs;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MoonBunny
{
    public class InputManager : MonoBehaviour
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
            print("IngameInput Enabled");
            InputAsset.Ingame.Enable();
        }

        public void DisableIngameInput()
        {
            print("IngameInput Disabled");
            InputAsset.Ingame.Disable();
        }
    }
}