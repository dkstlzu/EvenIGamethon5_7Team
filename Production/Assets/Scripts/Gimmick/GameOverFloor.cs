using System;
using MoonBunny.Dev;
using UnityEngine;

namespace MoonBunny
{
    public class GameOverFloor : Platform
    {
        public override void Invoke(MoonBunnyRigidbody with)
        {
            base.Invoke(with);
            
            if (with.tag == "Player")
            {
                Character character = with.GetComponent<Character>();
                if (character.FirstJumped)
                {
                    GameManager.instance.GameOver();
                }
            }
        }
    }
}