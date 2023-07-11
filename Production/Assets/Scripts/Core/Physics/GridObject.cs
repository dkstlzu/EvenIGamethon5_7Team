using System;
using TreeEditor;
using UnityEngine;

namespace MoonBunny
{
    public class GridObject : MonoBehaviour
    {
        public GridTransform GridTransform;

        protected virtual void Reset()
        {
            GridTransform = new GridTransform(transform);
        }

        protected virtual void Update()
        {
            GridTransform.Update();
        }
    }
}