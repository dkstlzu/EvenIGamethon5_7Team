using UnityEngine;

namespace MoonBunny
{
    public class ItemBox : Gimmick
    {
        public GameObject HeartPrefab;
        [Range(0, 1)] public float HeartPotential;
        public GameObject StarCandyPrefab;
        [Range(0, 1)] public float StarCandyPotential;
        public GameObject RicecakePrefab;
        [Range(0, 1)] public float RicecakePotential;

        public Transform SpawnPoint;

        private float _totalPotential => HeartPotential + StarCandyPotential + RicecakePotential;

        public override void Invoke(MoonBunnyRigidbody rigidbody)
        {
            base.Invoke(rigidbody);
            
            float randomValue = Random.value;
            GameObject targetGo = null;
            Transform parent = GameObject.FindWithTag("Items").transform;

            if (randomValue < HeartPotential / _totalPotential)
            {
                targetGo = HeartPrefab;
            } else if (randomValue >= HeartPotential / _totalPotential && randomValue < (HeartPotential + (StarCandyPotential) / _totalPotential))
            {
                targetGo = StarCandyPrefab;
            } else
            {
                targetGo = RicecakePrefab;
            }

            if (targetGo != null)
            {
                Instantiate(targetGo, SpawnPoint.position, Quaternion.identity, parent);
            }
            else
            {
                print("Something wrong on itembox");
            }
            
            Destroy(gameObject);
        }
    }
}