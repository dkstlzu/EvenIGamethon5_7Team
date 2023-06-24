using System;
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

        public void Block()
        {
            
        }
    }
}