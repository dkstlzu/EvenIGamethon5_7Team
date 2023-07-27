using System;
using System.Collections.Generic;
using dkstlzu.Utility;
using UnityEngine;

namespace MoonBunny
{
    public interface IUpdatable
    {
        void Update(float delta);
    }

    public class TimeUpdatable : IUpdatable, IEquatable<TimeUpdatable>, IEquatable<IUpdatable>
    {
        public static float GlobalSpeed = 1;
        public static bool Enabled;

        [RuntimeInitializeOnLoadMethod]
        static void Init()
        {
            GlobalSpeed = 1;
            Enabled = true;
        }

        public static void Stop()
        {
            Enabled = false;
        }

        public static void Resume()
        {
            Enabled = true;
        }
        
        
        private readonly IUpdatable _updatable;
        private float _speed;
        
        public TimeUpdatable(IUpdatable updatable, float speed)
        {
            _updatable = updatable;
            _speed = speed;
        }

        public void Update(float delta)
        {
            if (!Enabled) return;

            _updatable.Update(delta * _speed * GlobalSpeed);
        }

        public bool Equals(IUpdatable other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(_updatable, other)) return true;
            return Equals(_updatable, other);
        }
        
        public bool Equals(TimeUpdatable other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(_updatable, other._updatable);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType() && obj.GetType() != typeof(IUpdatable)) return false;

            if (obj.GetType() == typeof(IUpdatable)) return Equals((IUpdatable)obj);
            return Equals((TimeUpdatable)obj);
        }

        public override int GetHashCode()
        {
            return (_updatable != null ? _updatable.GetHashCode() : 0);
        }
    }

    public class UpdateManager : Singleton<UpdateManager>
    {
        private List<IUpdatable> _updatableList = new List<IUpdatable>();

        public void Register(IUpdatable updatable, float timeSpeed = 1)
        {
            if (timeSpeed != 1)
            {
                Register(new TimeUpdatable(updatable, timeSpeed));
            }
            else
            {
                _updatableList.Add(updatable);
            }
        }

        public void Register(TimeUpdatable timeUpdatable)
        {
            _updatableList.Add(timeUpdatable);
        }

        public void Unregister(IUpdatable updatable)
        {
            _updatableList.Remove(updatable);
        }

        private void Update()
        {
            float delta = Time.deltaTime;
            foreach (IUpdatable updatable in _updatableList)
            {
                updatable.Update(delta);
            }
        }
    }
}