using System;
using EvenI7.ProtoScreenSplit;
using UnityEngine;

namespace EvenI7
{
    public class Obstacle : MonoBehaviour
    {
        public Collider2D Collider;
        public int Damage;
        private void Start()
        {
            Collider = GetComponent<Collider2D>();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.tag == "Player")
            {
                other.GetComponent<Bouncable>().BounceFrom(transform.position);
            }
        }

        public void Block()
        {
            
        }
    }
}