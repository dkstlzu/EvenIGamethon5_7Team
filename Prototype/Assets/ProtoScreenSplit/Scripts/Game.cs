using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace EvenI7.ProtoScreenSplit
{
    public class Game : Singleton<Game>
    {
        public int Score;
        public Transform StartPosition;
        public GUIStyle FrameGUIStyle;

        private void Awake()
        {
            Item.OnItemTaken += (score) =>
            {
                Score += score;
            };
        }

        private void Start()
        {
            Application.targetFrameRate = 60;
        }

        public void Restart()
        {
            SceneManager.LoadScene(0);
        }

        private const float FrameUpdateInterval = 1;
        private float _timer;
        private int intFrame;

        private void OnGUI()
        {
            _timer += Time.deltaTime;

            if (_timer > FrameUpdateInterval)
            {
                _timer = 0;
                            
                float frame = 1 / Time.deltaTime;
                intFrame = (int)frame;
            }
            
            GUI.TextArea(new Rect(50, 50, 300, 100), intFrame.ToString()+"fps", FrameGUIStyle);

        }
    }
}