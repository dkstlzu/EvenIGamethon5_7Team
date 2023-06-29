using System;
using UnityEngine;

namespace EvenI7.Proto_Swiping
{
    public class Swiping : MonoBehaviour
    {
        public LineRenderer LineRenderer;

        public Vector2 StartPosition;
        public Vector2 EndPosition;

        public void SetUp(Vector2 start, Vector2 end)
        {
            StartPosition = start;
            EndPosition = end;

            Vector3[] linePositions = {StartPosition, EndPosition};
            LineRenderer.SetPositions(linePositions);
        }
    }
}