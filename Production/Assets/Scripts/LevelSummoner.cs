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
        public float SummonPositionVariation = 0.1f;
        private List<Vector2Int> _summonPositionList = new List<Vector2Int>();
        
        private int xmin => GridTransform.GridXMin;
        private int xmax => GridTransform.GridXMax;
        private int ymin = 0;
        private int ymax => MaxGridHeight;
        
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
            int summonNumber = 0;

            GameObject parent = GameObject.FindWithTag("Items");

            if (parent == null)
            {
                parent = new GameObject("Ricecakes");
            }

            while (summonNumber < RicecakeNumber)
            {
                Vector2Int randomGridPosition = GetRandomGridPosition();

                if (!_summonPositionList.Contains(randomGridPosition) && !GridTransform.HasGridObject(randomGridPosition))
                {
                    Vector3 summonPosition = GetRealPositionWithVariation(randomGridPosition, GridTransform.GridSetting, SummonPositionVariation);
                    MonoBehaviour.Instantiate(RicecakePrefab, summonPosition, Quaternion.identity, parent.transform);
                    
                    _summonPositionList.Add(randomGridPosition);
                    summonNumber++;
                }
            }
        }

        public void SummonCoins()
        {
            int summonNumber = 0;

            GameObject parent = GameObject.FindWithTag("Items");

            if (parent == null)
            {
                parent = new GameObject("Coins");
            }
            
            while (summonNumber < CoinNumber)
            {
                Vector2Int randomGridPosition = GetRandomGridPosition();

                if (!_summonPositionList.Contains(randomGridPosition) && !GridTransform.HasGridObject(randomGridPosition))
                {
                    Vector3 summonPosition = GetRealPositionWithVariation(randomGridPosition, GridTransform.GridSetting, SummonPositionVariation);
                    MonoBehaviour.Instantiate(CoinPrefab, summonPosition, Quaternion.identity, parent.transform);
                    
                    _summonPositionList.Add(randomGridPosition);
                    summonNumber++;
                }
            }
        }

        public void SummonFriendCollectables()
        {
            int summonNumber = 0;

            GameObject parent = GameObject.FindWithTag("Friends");

            if (parent == null)
            {
                parent = new GameObject("FriendCollectables");
            }
            
            while (summonNumber < FriendCollectableNumber)
            {
                Vector2Int randomGridPosition = GetRandomGridPosition();

                if (!_summonPositionList.Contains(randomGridPosition) && !GridTransform.HasGridObject(randomGridPosition))
                {
                    Vector3 summonPosition = GetRealPositionWithVariation(randomGridPosition, GridTransform.GridSetting, SummonPositionVariation);
                    MonoBehaviour.Instantiate(FriendPrefab, summonPosition, Quaternion.identity, parent.transform);

                    _summonPositionList.Add(randomGridPosition);
                    summonNumber++;
                }
            }
        }

        private Vector2Int GetRandomGridPosition()
        {
            int randomX = Random.Range(xmin, xmax + 1);
            int randomY = Random.Range(ymin, ymax + 1);

            return new Vector2Int(randomX, randomY);
        }

        private Vector3 GetRealPositionWithVariation(Vector2Int gridPosition, GridSetting gridSetting, float variation)
        {
            return GridTransform.ToReal(gridPosition) + GetSummonVariation(gridSetting, variation);
        }

        private Vector2 GetSummonVariation(GridSetting gridSetting, float variation)
        {
            return new Vector2(Random.Range(-gridSetting.GridWidth * variation, gridSetting.GridWidth * variation),
                Random.Range(-gridSetting.GridHeight * variation, gridSetting.GridHeight * variation));
        }
    }
}