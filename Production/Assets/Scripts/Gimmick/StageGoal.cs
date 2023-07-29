using System;
using UnityEngine;

namespace MoonBunny
{
    public class StageGoal : Gimmick
    {
        public Stage Stage;

        public override bool Invoke(MoonBunnyRigidbody with, MoonBunnyCollider.Direction direction)
        {
            if (!base.Invoke(with, direction)) return false;

            Stage.Clear();
            return true;
        }
    }
}