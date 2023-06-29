using System;
using UnityEngine;

namespace MoonBunny
{
    public class LevelSummoner : MonoBehaviour
    {
        [Header("Prefabs")]
        public GameObject ItemPrefab;
        public GameObject ObstaclePrefab;
        public GameObject PlatformPrefab;
        public GameObject FriendPrefab;

        [Header("Parent GameObjects")] 
        public GameObject ItemParent;
        public GameObject ObstacleParent;
        public GameObject PlatformParent;
        public GameObject FriendParent;

        private Character _player;

        private void Awake()
        {
            _player = GameObject.FindWithTag("Player").GetComponent<Character>();
        }
    }
}