using System;
using UnityEngine;

namespace MoonBunny
{
    public class StageGoal : Gimmick
    {
        public Stage Stage;

        public override void Invoke(MoonBunnyRigidbody with)
        {
            base.Invoke(with);
            
            Stage.Clear();
        }
    }
}