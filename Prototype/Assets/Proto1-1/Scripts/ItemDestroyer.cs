using System;
using UnityEngine;

namespace EvenI7.Proto1_1
{
    public class ItemDestroyer : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Item") ||
                other.gameObject.layer == LayerMask.NameToLayer("Obstacle") ||
                other.gameObject.layer == LayerMask.NameToLayer("Platform"))
            {
                Destroy(other.gameObject);
            }
        }
    }
}