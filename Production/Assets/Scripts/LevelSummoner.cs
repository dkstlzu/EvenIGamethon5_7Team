using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace MoonBunny
{
    public class LevelSummoner : MonoBehaviour
    {
        [Header("Prefabs")]
        public GameObject RicecakePrefab;
        public GameObject FriendPrefab;

        public int RicecakeNumber;
        [Range(0, 1)] public float RainbowRicecakeRatio;

        private Character _player;
        private Stage _stage;

        private void Awake()
        {
            _player = GameObject.FindWithTag("Player").GetComponent<Character>();
            _stage = GetComponent<Stage>();
        }

        public void Summon(GameObject go, Vector3 position)
        {
            Instantiate(go, position, Quaternion.identity);
        }

        public void SummonRicecakes()
        {
            int xmin = GridTransform.GridXMin;
            int xmax = GridTransform.GridXMax;
            int ymin = 0;
            int ymax = _stage.EndlineHeight;

            int summonNumber = 0;
            List<Vector2Int> summonPositionList = new List<Vector2Int>();

            float startTime = Time.realtimeSinceStartup;
            float nowTime = Time.realtimeSinceStartup;
            float crash = 2f;

            while (summonNumber < RicecakeNumber)
            {
                int randomX = Random.Range(xmin, xmax + 1);
                int randomY = Random.Range(ymin, ymax + 1);

                Vector2Int randomGridPosition = new Vector2Int(randomX, randomY);

                if (!summonPositionList.Contains(randomGridPosition))
                {
                    summonPositionList.Add(randomGridPosition);
                    summonNumber++;
                }

                nowTime = Time.realtimeSinceStartup;

                if (nowTime - startTime > crash)
                {
                    Debug.LogWarning($"LevelSummoner ricecake summon failed by timeover {nowTime - startTime}");
                    break;
                }
            }

            GameObject parent = GameObject.FindWithTag("Items");

            if (parent == null)
            {
                parent = new GameObject("Ricecakes");
            }

            foreach (Vector2Int position in summonPositionList)
            {
                Ricecake cake = Instantiate(RicecakePrefab, GridTransform.ToReal(position), Quaternion.identity, parent.transform).GetComponent<Ricecake>();

                if (Random.value <= RainbowRicecakeRatio)
                {
                    cake.MakeRainbow();
                }
            }
        }
    }
}