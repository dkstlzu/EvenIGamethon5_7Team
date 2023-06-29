using System;
using UnityEngine;

namespace EvenI7.ProtoScreenSplit
{
    public class ScreenSplitLevelSummoner : MonoBehaviour
    {
        public GameObject ItemPrefab;
        public GameObject ObstaclePrefab;
        public GameObject PlatformPrefab;

        private ProtoScreenSplitCharacter _player;

        private void Awake()
        {
            _player = GameObject.FindWithTag("Player").GetComponent<ProtoScreenSplitCharacter>();
        }
    }
}