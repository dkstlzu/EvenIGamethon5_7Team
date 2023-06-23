using UnityEngine;

namespace EvenI7.Proto1_2
{
    public class SideWallBound : MonoBehaviour
    {
        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.tag == "Player")
            {
                var character = other.gameObject.GetComponent<Proto1_2Character>();
                if (character)
                    character.SwitchDirection();
            }
        }
    }
}