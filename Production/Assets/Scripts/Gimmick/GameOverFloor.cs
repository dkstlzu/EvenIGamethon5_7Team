using System;
using UnityEngine;

namespace MoonBunny
{
    public class GameOverFloor : MonoBehaviour
    {
        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.tag == "Player")
            {
                var character = other.gameObject.GetComponent<Character>();
                if (character.FirstJumped)
                {
                    ScreenSplit.instance.DestroyScreen(ScreenSplit.instance.GetSide(character.Friend));
                }
            }
        }
    }
}