using System;
using System.Collections.Generic;
using dkstlzu.Utility;
using MoonBunny.Effects;
using UnityEngine;
using Random = UnityEngine.Random;

namespace MoonBunny
{
    [Serializable]
    public class LevelSummonerComponent : IUpdatable
    {
        [Header("Prefabs")]
        public GameObject RicecakePrefab;
        public GameObject CoinPrefab;
        public GameObject FriendPrefab;

        public int RicecakeNumber;
        [Range(0, 1)] public float RainbowRicecakeRatio;
        public int CoinNumber;
        public int FriendCollectableNumber;

        [HideInInspector] public bool SummonThunderEnable; 
        public bool SummonThunderEnabled;
        public float SummonThunderInterval;
        private float _thunderTimer;
        public float ThunderWarningTime;
            
        [HideInInspector] public bool SummonShootingStarEnable; 
        public bool SummonShootingStarEnabled;
        public float SummonShootingStarInterval;
        private float _shoootingStarTimer;
        public float ShootingStarWarningTime;
        
        public int MaxGridHeight;
        private List<Vector2Int> _summonPositionList = new List<Vector2Int>();
        
        public void Update(float delta)
        {
            if (SummonThunderEnable)
            {
                SummonThunder(delta);
            }

            if (SummonShootingStarEnable)
            {
                SummonShootingStar(delta);
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
                int targetGridX = Random.Range(GridTransform.GridXMin + 1, GridTransform.GridXMax - 1);
                
                Vector2 areaMin = GridTransform.ToReal(new Vector2Int(targetGridX - 1, GridTransform.GridYMin)) - GridTransform.GetGridSize() / 2;
                Vector2 areaMax = GridTransform.ToReal(new Vector2Int(targetGridX + 1, GameManager.instance.Stage.Spec.Height)) + GridTransform.GetGridSize() / 2;

                WarningEffect warningEffect = new WarningEffect(new Rect(areaMin, areaMax - areaMin), ShootingStarWarningTime);
                warningEffect.Effect();
                
                UpdateManager.instance.Delay(() =>
                {
                    Character character = GameObject.FindWithTag("Player").GetComponent<Character>();
                    Vector2 targetPoint = new Vector2(targetGridX, (character.transform.position + Vector3.up * Camera.main.orthographicSize + Vector3.up * 4).y);
                    Vector2Int targetGrid = GridTransform.ToGrid(targetPoint);
                    new ShootingStarEffect(targetGrid.y, targetGridX).Effect();
                }, ShootingStarWarningTime);
                _shoootingStarTimer = 0;
            }
        }
        
        public void SummonRicecakes()
        {
            int xmin = GridTransform.GridXMin;
            int xmax = GridTransform.GridXMax;
            int ymin = 0;
            int ymax = MaxGridHeight;

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
                    Ricecake cake = MonoBehaviour.Instantiate(RicecakePrefab, GridTransform.ToReal(randomGridPosition), Quaternion.identity, parent.transform).GetComponent<Ricecake>();

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
            int ymax = MaxGridHeight;
            
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
                    Coin coin = MonoBehaviour.Instantiate(CoinPrefab, GridTransform.ToReal(randomGridPosition), Quaternion.identity, parent.transform).GetComponent<Coin>();
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
            int ymax = MaxGridHeight;
            
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
                    FriendCollectable collectable = MonoBehaviour.Instantiate(FriendPrefab, GridTransform.ToReal(randomGridPosition), Quaternion.identity, parent.transform).GetComponent<FriendCollectable>();
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

        [HideInInspector] public bool SummonThunderEnable; 
        public bool SummonThunderEnabled;
        public float SummonThunderInterval;
        private float _thunderTimer;
        public float ThunderWarningTime;
        
        [HideInInspector] public bool SummonShootingStarEnable; 
        public bool SummonShootingStarEnabled;
        public float SummonShootingStarInterval;
        private float _shoootingStarTimer;
        public float ShootingStarWarningTime;

        private int MaxGridHeight;
        private List<Vector2Int> _summonPositionList = new List<Vector2Int>();

        private void Update()
        {
            if (SummonThunderEnable)
            {
                SummonThunder(Time.deltaTime);
            }

            if (SummonShootingStarEnable)
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
                int targetGridX = Random.Range(GridTransform.GridXMin + 1, GridTransform.GridXMax - 1);
                
                Vector2 areaMin = GridTransform.ToReal(new Vector2Int(targetGridX - 1, GridTransform.GridYMin)) - GridTransform.GetGridSize() / 2;
                Vector2 areaMax = GridTransform.ToReal(new Vector2Int(targetGridX + 1, GameManager.instance.Stage.Spec.Height)) + GridTransform.GetGridSize() / 2;

                WarningEffect warningEffect = new WarningEffect(new Rect(areaMin, areaMax - areaMin), ShootingStarWarningTime);
                warningEffect.Effect();
                
                UpdateManager.instance.Delay(() =>
                {
                    Character character = GameObject.FindWithTag("Player").GetComponent<Character>();
                    Vector2 targetPoint = new Vector2(targetGridX, (character.transform.position + Vector3.up * Camera.main.orthographicSize + Vector3.up * 4).y);
                    Vector2Int targetGrid = GridTransform.ToGrid(targetPoint);
                    new ShootingStarEffect(targetGrid.y, targetGridX).Effect();
                }, ShootingStarWarningTime);
                _shoootingStarTimer = 0;
            }
        }
        
        public void SummonRicecakes()
        {
            int xmin = GridTransform.GridXMin;
            int xmax = GridTransform.GridXMax;
            int ymin = 0;
            int ymax = MaxGridHeight;

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
            int ymax = MaxGridHeight;
            
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
            int ymax = MaxGridHeight;
            
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