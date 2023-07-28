﻿using System;
using UnityEngine;

namespace MoonBunny.UIs
{
    public class TutorialUI : UI
    {
        private void Awake()
        {
            Time.timeScale = 0;
        }

        public void OnConfirm()
        {
            Time.timeScale = 1;
            Destroy(gameObject);
        }
    }
}