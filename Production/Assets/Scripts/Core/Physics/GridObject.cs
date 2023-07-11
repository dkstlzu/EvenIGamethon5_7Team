using System;
using TreeEditor;
using UnityEngine;

namespace MoonBunny
{
    [ExecuteInEditMode]
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

        [ContextMenu("SnapToGrid")]
        public void SnapToGrid()
        {
            GridTransform.SnapToGrid();
        }
    }
}