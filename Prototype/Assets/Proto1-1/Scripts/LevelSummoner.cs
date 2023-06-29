using System;
using EvenI7.Proto1_2;
using UnityEngine;
using Random = UnityEngine.Random;

namespace EvenI7.Proto1_1
{
    public class LevelSummoner : MonoBehaviour
    {
        public GameObject ItemPrefab;
        public GameObject ObstaclePrefab;
        public GameObject PlatformPrefab;
        public SpawnArea SpawnArea;
        
        public float ItemSpawnIntervalHeight;
        public float ObstacleSpawnIntervalHeight;
        public float PlatformSpawnIntervalHeight;
        private float _lastSpawnedItemHeight;
        private float _lastSpawnedObstacleHeight;
        private float _lastSpawnedPlatformHeight;

        public Transform LastPlatformPosition;
        public int AcceleratingHeightInterval;
        
        private GameObject _player;

        private void Start()
        {
            _player = GameObject.FindWithTag("Player");
        }

        private void Update()
        {
            if (ItemPrefab)
            if (_player.transform.position.y > _lastSpawnedItemHeight + ItemSpawnIntervalHeight)
            {
                Instantiate(ItemPrefab, SpawnArea.GetRandomPoint(), Quaternion.identity);
                _lastSpawnedItemHeight += ItemSpawnIntervalHeight;
            }

            if (ObstaclePrefab)
            if (_player.transform.position.y > _lastSpawnedObstacleHeight + ObstacleSpawnIntervalHeight)
            {
                Instantiate(ObstaclePrefab, SpawnArea.GetRandomPoint(), Quaternion.identity);
                _lastSpawnedObstacleHeight += ObstacleSpawnIntervalHeight;
            }
            
            if (PlatformPrefab)
            if (_player.transform.position.y > _lastSpawnedPlatformHeight + PlatformSpawnIntervalHeight)
            {
                Vector3 spawnPosition = new Vector3( LastPlatformPosition.position.x + Random.Range(-1.5f, 1.5f), LastPlatformPosition.position.y + Random.Range(1f, 2), 0);
                spawnPosition = new Vector3(Mathf.Clamp(spawnPosition.x, -SpawnArea.Area.width/2, SpawnArea.Area.width/2), spawnPosition.y, 0);
                BouncyPlatform bouncyPlatform = Instantiate(PlatformPrefab, spawnPosition, Quaternion.identity).GetComponent<BouncyPlatform>();
                bouncyPlatform.JumpPower += (int)(_player.transform.position.y / AcceleratingHeightInterval);
                _lastSpawnedPlatformHeight += bouncyPlatform.transform.position.y - LastPlatformPosition.position.y;
                LastPlatformPosition = bouncyPlatform.transform;
            }
        }
    }
}