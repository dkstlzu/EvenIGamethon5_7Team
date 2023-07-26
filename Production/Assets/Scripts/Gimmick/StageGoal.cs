using System;
using UnityEngine;

namespace MoonBunny
{
    public class StageGoal : Gimmick
    {
        public Stage Stage;

        public override bool Invoke(MoonBunnyRigidbody with)
        {
            if (!base.Invoke(with)) return false;

            Stage.Clear();
            return true;
        }
    }
}