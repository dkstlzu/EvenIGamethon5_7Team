using System;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor.SceneManagement;
#endif

namespace MoonBunny
{
    [ExecuteAlways]
    public class GridObject : FieldObject
    {
        [HideInInspector] public GridTransform GridTransform;

        protected virtual void Reset()
        {
            GridTransform = new GridTransform(transform);
        }

        protected virtual void Awake()
        {
            if (GridTransform == null)
            {
                GridTransform = new GridTransform(transform);
            } else if (GridTransform.transform == null)
            {
                GridTransform.transform = transform;
            }
        }

        protected virtual void Update()
        {
#if UNITY_EDITOR
            if (PrefabStageUtility.GetCurrentPrefabStage() != null)
            {
                return;
            }
#endif
            GridTransform.Update();
        }

        [ContextMenu("SnapToGrid")]
        public void SnapToGrid()
        {
            GridTransform.SnapToGrid();
        }

        [ContextMenu("Move To Origin")]
        public void ToOrigin()
        {
            transform.position = GridTransform.ToReal(Vector2Int.zero);
        }
    }
}