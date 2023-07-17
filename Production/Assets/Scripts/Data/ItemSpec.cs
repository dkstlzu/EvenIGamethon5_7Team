﻿using UnityEngine;

namespace MoonBunny
{
    [CreateAssetMenu(menuName = "Specs/Item")]
    public class ItemSpec : ScriptableObject
    {
        public int Score;
        public AudioClip AudioClip;
		public Sprite Sprite;
    }
}