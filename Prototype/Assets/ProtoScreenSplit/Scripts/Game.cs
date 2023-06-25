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

        private void Awake()
        {
            Item.OnItemTaken += (score) =>
            {
                Score += score;
            };
        }

        public void Restart()
        {
            SceneManager.LoadScene(0);
        }
    }
}