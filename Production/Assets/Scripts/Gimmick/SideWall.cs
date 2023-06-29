using System;
using System.Collections.Generic;
using dkstlzu.Utility;
using UnityEngine;

namespace MoonBunny
{
    public class SideWall : MonoBehaviour
    {
        public List<EventTrigger> EventTriggers;

        private void Awake()
        {
            foreach (EventTrigger trigger in EventTriggers)
            {
                trigger.AddEnterGOEvent(OnCollision);
            }
        }

        public void OnCollision(GameObject go)
        {
            Bouncable bouncable;
            if (go.TryGetComponent<Bouncable>(out bouncable))
            {
                bouncable.BounceX();
            }
        }
    }
}