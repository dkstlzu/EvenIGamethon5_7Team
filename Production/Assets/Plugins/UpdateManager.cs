using System;
using System.Collections.Generic;
using UnityEngine;

namespace dkstlzu.Utility
{
    public interface IUpdatable
    {
        void Update(float delta);
    }

    public class TimeUpdatable : IUpdatable
    {
        public static float GlobalSpeed = 1;
        public static bool Enabled;

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

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;

            if (obj is TimeUpdatable timeUpdatable)
            {
                return _updatable.Equals(timeUpdatable._updatable);
            } else if (obj is IUpdatable updatable)
            {
                return _updatable.Equals(updatable);
            }

            return false;
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

        class DelayedAction : IUpdatable
        {
            private Action _action;
            private float _time;

            private TimeUpdatable _timeUpdatable;
            
            public DelayedAction(Action action, float time)
            {
                _action = action;
                _time = time;

                _timeUpdatable = new TimeUpdatable(this, 1);
                
                instance.Register(_timeUpdatable);
            }

            public void Update(float delta)
            {
                _time -= delta;

                if (_time <= 0)
                {
                    _action?.Invoke();
                    instance.Unregister(_timeUpdatable);
                }
            }
        }
        
        class WhileAction : IUpdatable
        {
            private Action<float> _whileAction;
            private Action _finishAction;
            private float _time;

            private TimeUpdatable _timeUpdatable;
            
            public WhileAction(Action<float> whileAction, Action finishAction, float time)
            {
                _whileAction = whileAction;
                _finishAction = finishAction;
                _time = time;

                _timeUpdatable = new TimeUpdatable(this, 1);
                instance.Register(_timeUpdatable);
            }

            public void Update(float delta)
            {
                _time -= delta;
                
                _whileAction?.Invoke(_time);

                if (_time <= 0)
                {
                    _finishAction?.Invoke();
                    instance.Unregister(_timeUpdatable);
                }
            }
        }
        
        public void Delay(Action action, float delay)
        {
            new DelayedAction(action, delay);
        }
        
        public void Delay(Action<float> whileAction, Action finishAction, float delay)
        {
            new WhileAction(whileAction, finishAction, delay);
        }

        private void Update()
        {
            float delta = Time.deltaTime;
            for (int i = 0; i < _updatableList.Count; i++)
            {
                if (_updatableList[i] == null)
                {
                    _updatableList.RemoveAt(i);
                    i--;
                    continue;
                }

                try
                {
                    _updatableList[i].Update(delta);
                }
                catch (Exception e)
                {
                    Debug.Log(e + "\n" + "UpdateManager error. auto remove element");
                    _updatableList.RemoveAt(i);
                    i--;
                }
            }
        }
    }
}