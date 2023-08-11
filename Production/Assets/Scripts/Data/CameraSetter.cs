﻿using System;
using Cinemachine;
using dkstlzu.Utility;
using UnityEngine;
using UnityEngine.Serialization;

namespace MoonBunny
{
    public class CameraSetter : MonoBehaviour
    {
        public Camera MainCamera;
        public AudioListener AudioListener;
        public CinemachineBrain Brain;
        public CinemachineVirtualCamera VirtualCamera;
        public CinemachineConfiner2D Confiner2D;

        public event Action OnCameraSetFinish;
        public float CameraSetTime;
        
        protected void Start()
        {
            AudioListener.volume = GameManager.instance.VolumeSetting;
            Confiner2D.m_BoundingShape2D = GameManager.instance.Stage.LevelConfiner;
            Confiner2D.InvalidateCache();
            
            CoroutineHelper.OnNextFrame(() =>
            {
                OnCameraSetFinish?.Invoke();
                if (GameManager.instance.ShowTutorial)
                {
                    GameManager.instance.Stage.UI.TutorialOn();
                }
                else
                {
                    GameManager.instance.Stage.UI.GetComponent<Animator>().enabled = true;
                }
            });
        }
    }
}