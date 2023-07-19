﻿using UnityEngine;

namespace MoonBunny
{
    [CreateAssetMenu(fileName = "StageSpec", menuName = "Specs/StageSpec", order = 0)]
    public class StageSpec : ScriptableObject
    {
        public int Height;
        public int FirstStepScore;
        public int SecondStepScore;
        public int ThirdStepScore;
    }
}