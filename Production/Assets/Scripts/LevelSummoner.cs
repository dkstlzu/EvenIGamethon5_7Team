using System;
using System.Collections.Generic;
using dkstlzu.Utility;
using MoonBunny.Effects;
using UnityEngine;
using Random = UnityEngine.Random;

namespace MoonBunny
{
    public class LevelSummoner : MonoBehaviour
    {
        [Header("Prefabs")]
        public GameObject RicecakePrefab;
        public GameObject CoinPrefab;
        public GameObject FriendPrefab;

        public int RicecakeNumber;
        [Range(0, 1)] public float RainbowRicecakeRatio;
        public int CoinNumber;
        public int FriendCollectableNumber;

        public bool SummonThunderEnabled;
        public float SummonThunderInterval;
        private float _thunderTimer;
        public float ThunderWarningTime;
        
        public bool SummonShootingStarEnabled;
        public float SummonShootingStarInterval;
        private float _shoootingStarTimer;

        private Character _player;
        private Stage _stage;
        private List<Vector2Int> _summonPositionList = new List<Vector2Int>();

        private void Awake()
        {
            _player = GameObject.FindWithTag("Player").GetComponent<Character>();
            _stage = GetComponent<Stage>();
        }

        private void Update()
        {
            if (SummonThunderEnabled)
            {
                SummonThunder(Time.deltaTime);
            }

            if (SummonShootingStarEnabled)
            {
                SummonShootingStar(Time.deltaTime);
            }
        }

        private void SummonThunder(float deltaTime)
        {
            _thunderTimer += deltaTime;

            if (_thunderTimer >= SummonThunderInterval)
            {
                new ThunderEffect(Random.Range(GridTransform.GridXMin, GridTransform.GridXMax), ThunderWarningTime).Effect();
                _thunderTimer = 0;
            }
        }

        private void SummonShootingStar(float deltaTime)
        {
            _shoootingStarTimer += deltaTime;

            if (_shoootingStarTimer >= SummonShootingStarInterval)
            {
                Vector2Int targetGrid = GridTransform.ToGrid(_player.transform.position + Vector3.up * Camera.main.orthographicSize);
                new ShootingStarEffect(targetGrid.y, Random.Range(GridTransform.GridXMin + 1, GridTransform.GridXMax - 1)).Effect();
                _shoootingStarTimer = 0;
            }
        }
        
        public void SummonRicecakes()
        {
            int xmin = GridTransform.GridXMin;
            int xmax = GridTransform.GridXMax;
            int ymin = 0;
            int ymax = _stage.Spec.Height;

            int summonNumber = 0;

            float startTime = Time.realtimeSinceStartup;
            float nowTime = Time.realtimeSinceStartup;
            float crash = 2f;
            
            GameObject parent = GameObject.FindWithTag("Items");

            if (parent == null)
            {
                parent = new GameObject("Ricecakes");
            }

            while (summonNumber < RicecakeNumber)
            {
                int randomX = Random.Range(xmin, xmax + 1);
                int randomY = Random.Range(ymin, ymax + 1);

                Vector2Int randomGridPosition = new Vector2Int(randomX, randomY);

                if (!_summonPositionList.Contains(randomGridPosition) && !GridTransform.HasGridObject(randomGridPosition))
                {
                    _summonPositionList.Add(randomGridPosition);
                    summonNumber++;
                    Ricecake cake = Instantiate(RicecakePrefab, GridTransform.ToReal(randomGridPosition), Quaternion.identity, parent.transform).GetComponent<Ricecake>();

                    if (Random.value <= RainbowRicecakeRatio)
                    {
                        cake.MakeRainbow();
                    }
                }

                nowTime = Time.realtimeSinceStartup;

                if (nowTime - startTime > crash)
                {
                    Debug.LogWarning($"LevelSummoner ricecake summon failed by timeover {nowTime - startTime}");
                    break;
                }
            }
        }

        public void SummonCoins()
        {
            int xmin = GridTransform.GridXMin;
            int xmax = GridTransform.GridXMax;
            int ymin = 0;
            int ymax = _stage.Spec.Height;
            
            int summonNumber = 0;

            float startTime = Time.realtimeSinceStartup;
            float nowTime = Time.realtimeSinceStartup;
            float crash = 2f;
            
            GameObject parent = GameObject.FindWithTag("Items");

            if (parent == null)
            {
                parent = new GameObject("Coins");
            }
            
            while (summonNumber < CoinNumber)
            {
                int randomX = Random.Range(xmin, xmax + 1);
                int randomY = Random.Range(ymin, ymax + 1);

                Vector2Int randomGridPosition = new Vector2Int(randomX, randomY);

                if (!_summonPositionList.Contains(randomGridPosition) && !GridTransform.HasGridObject(randomGridPosition))
                {
                    _summonPositionList.Add(randomGridPosition);
                    summonNumber++;
                    Coin coin = Instantiate(CoinPrefab, GridTransform.ToReal(randomGridPosition), Quaternion.identity, parent.transform).GetComponent<Coin>();
                }

                nowTime = Time.realtimeSinceStartup;

                if (nowTime - startTime > crash)
                {
                    Debug.LogWarning($"LevelSummoner coin summon failed by timeover {nowTime - startTime}");
                    break;
                }
            }
        }

        public void SummonFriendCollectables()
        {
            int xmin = GridTransform.GridXMin;
            int xmax = GridTransform.GridXMax;
            int ymin = 0;
            int ymax = _stage.Spec.Height;
            
            int summonNumber = 0;

            float startTime = Time.realtimeSinceStartup;
            float nowTime = Time.realtimeSinceStartup;
            float crash = 2f;
            
            GameObject parent = GameObject.FindWithTag("Friends");

            if (parent == null)
            {
                parent = new GameObject("FriendCollectables");
            }
            
            while (summonNumber < FriendCollectableNumber)
            {
                int randomX = Random.Range(xmin, xmax + 1);
                int randomY = Random.Range(ymin, ymax + 1);

                Vector2Int randomGridPosition = new Vector2Int(randomX, randomY);

                if (!_summonPositionList.Contains(randomGridPosition) && !GridTransform.HasGridObject(randomGridPosition))
                {
                    _summonPositionList.Add(randomGridPosition);
                    summonNumber++;
                    FriendCollectable collectable = Instantiate(FriendPrefab, GridTransform.ToReal(randomGridPosition), Quaternion.identity, parent.transform).GetComponent<FriendCollectable>();
                }

                nowTime = Time.realtimeSinceStartup;

                if (nowTime - startTime > crash)
                {
                    Debug.LogWarning($"LevelSummoner coin summon failed by timeover {nowTime - startTime}");
                    break;
                }
            }
        }
    }
}