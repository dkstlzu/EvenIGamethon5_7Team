using System;
using MoonBunny.Dev;
using UnityEngine;

namespace MoonBunny
{
    public class GameOverFloor : Platform
    {
        public override bool Invoke(MoonBunnyRigidbody with, MoonBunnyCollider.Direction direction)
        {
            if (!base.Invoke(with, direction)) return false;

            if (with.tag == "Player")
            {
                Character character = with.GetComponent<Character>();
                if (character.FirstJumped)
                {
                    character.Hit(null);
                    character.Hit(null);
                    character.Hit(null);
                    GameManager.instance.Stage.Fail();
                    return true;
                }
            }
            else
            {
                Destroy(with.gameObject);
            }

            return false;
        }
    }
}