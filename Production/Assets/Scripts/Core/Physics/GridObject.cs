using System;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif

namespace MoonBunny
{
    [ExecuteAlways]
    public class GridObject : FieldObject
    {
        public GridTransform GridTransform;

        protected virtual void Reset()
        {
            GridTransform = new GridTransform(transform);
        }

        protected virtual void Awake()
        {
#if UNITY_EDITOR
            if (!EditorApplication.isPlaying) return;
#endif
            if (GridTransform.transform == null)
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
#if UNITY_EDITOR
            Undo.RecordObject(this, "GridObjectSnapToGrid");
#endif
            GridTransform.SnapToGrid();
        }
        
        [ContextMenu("UpdateGrid")]
        public void UpdateGrid()
        {
            var method = GridTransform.SnapMethod;
            GridTransform.SnapMethod = SnapMethod.RealToGrid;
            GridTransform.Update();
            GridTransform.SnapMethod = method;
#if UNITY_EDITOR
            EditorUtility.SetDirty(this);
#endif

        }

        [ContextMenu("Move To Origin")]
        public void ToOrigin()
        {
#if UNITY_EDITOR
            Undo.RecordObject(transform, "GridObjectToOrigin");
#endif
            transform.position = GridTransform.ToReal(Vector2Int.zero);
        }
    }
}