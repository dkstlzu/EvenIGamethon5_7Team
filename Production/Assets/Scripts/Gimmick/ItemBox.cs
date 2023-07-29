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
        [SerializeField] private AudioClip _audioClip;

        private float _totalPotential => HeartPotential + StarCandyPotential + RicecakePotential;

        public override bool Invoke(MoonBunnyRigidbody with, MoonBunnyCollider.Direction direction)
        {
            if (!base.Invoke(with, direction)) return false;

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
            
            SoundManager.instance.PlayClip(_audioClip);
            Destroy(gameObject);
            return true;
        }
    }
}