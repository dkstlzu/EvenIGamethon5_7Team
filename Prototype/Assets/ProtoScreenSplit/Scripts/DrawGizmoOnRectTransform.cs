using System;
using UnityEngine;

namespace EvenI7.ProtoScreenSplit
{
    public class DrawGizmoOnRectTransform : MonoBehaviour
    {
        private void OnDrawGizmosSelected()
        {
            RectTransform tr = transform as RectTransform;
            Gizmos.DrawCube(tr.position, tr.sizeDelta);
        }
    }
}