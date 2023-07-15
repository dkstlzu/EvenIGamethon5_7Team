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
            
            MoonBunnyLog.print("HI");

            if (with.tag == "Player")
            {
                MoonBunnyLog.print("HI Hello");
                Character character = with.GetComponent<Character>();
                if (character.FirstJumped)
                {
                    MoonBunnyLog.print("HI Hello World");
                    GameManager.instance.GameOver();
                }
            }
        }
    }
}