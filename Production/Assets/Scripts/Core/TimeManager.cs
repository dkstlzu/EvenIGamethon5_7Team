using System;
using UnityEngine;

namespace MoonBunny
{
    public class TimeManager : IUpdatable
    {
        private static TimeManager _instance;
        public static TimeManager instacne
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new TimeManager();
                }
                
                return _instance;
            }
        }

        private TimeManager()
        {
        }
        
        
        public void Update(float delta)
        {
            if (!_enabled) return;
            
            
        }

        private bool _enabled = true;

        public void Stop()
        {
            _enabled = false;
        }

        public void Resume()
        {
            _enabled = true;
        }

        public void Register(IUpdatable updatable, float timeSpeed)
        {
            
        }
    }
}