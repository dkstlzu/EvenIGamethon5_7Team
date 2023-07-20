using System;
using System.Collections.Generic;
using dkstlzu.Utility;
using UnityEngine;

namespace MoonBunny
{
    public interface IUpdatable
    {
        void Update();
    }

    public class UpdateManager : Singleton<UpdateManager>
    {
        private List<IUpdatable> _updatableList = new List<IUpdatable>();
        
        public void Register(IUpdatable updatable)
        {
            _updatableList.Add(updatable);
        }

        public void Unregister(IUpdatable updatable)
        {
            _updatableList.Remove(updatable);
        }

        private void Update()
        {
            foreach (IUpdatable updatable in _updatableList)
            {
                updatable.Update();
            }
        }
    }
}