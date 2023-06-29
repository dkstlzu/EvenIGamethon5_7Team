using System;
using UnityEngine;

namespace EvenI7.Proto1_1
{
    public class SideWallBounce : MonoBehaviour
    {
        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.tag == "Player")
            {
                other.gameObject.GetComponent<Proto1_1Character>().SideWallBounce();
            }
        }
    }
}