using System;
using UnityEngine;

namespace EvenI7.ProtoScreenSplit
{
    public class GameOverFloor : MonoBehaviour
    {
        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.tag == "Player")
            {
                var character = other.gameObject.GetComponent<ProtoScreenSplitCharacter>();
                if (character.FirstJump)
                {
                    ScreenSplit.instance.DestroyScreen(ScreenSplit.instance.GetSide(character.FriendCharacter));
                }
            }
        }
    }
}