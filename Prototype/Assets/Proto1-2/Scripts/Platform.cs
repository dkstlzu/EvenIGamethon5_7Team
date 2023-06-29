using System;
using UnityEngine;

namespace EvenI7.Proto1_2
{
    public class Platform : MonoBehaviour
    {
        public Collider2D Collider;
        private GameObject _player;

        private void Start()
        {
            _player = GameObject.FindWithTag("Player");
        }

        private void Update()
        {
            if (_player.transform.position.y < transform.position.y)
            {
                Collider.enabled = false;
            }
            else
            {
                Collider.enabled = true;
            }
        }
    }
}