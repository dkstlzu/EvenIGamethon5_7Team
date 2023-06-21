using UnityEngine;
using UnityEngine.Pool;

namespace EvenI7
{
    public class Item : MonoBehaviour
    {
        public int Score;
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private SpriteRenderer _renderer;

        public void Taken()
        {
            _audioSource.Play();
            _renderer.enabled = false;
            Destroy(gameObject, 2f);
        }
    }
}