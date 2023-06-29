using System;
using UnityEngine;

namespace EvenI7.ProtoScreenSplit
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class SideWall : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            Bouncable bouncable;
            if (other.TryGetComponent<Bouncable>(out bouncable))
            {
                bouncable.BounceX();
            }
        }
    }
}