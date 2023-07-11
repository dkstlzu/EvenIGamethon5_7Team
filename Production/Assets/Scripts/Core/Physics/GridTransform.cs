using System;
using MoonBunny.Dev;
using UnityEngine;

namespace MoonBunny
{
    [Serializable]
    public class GridTransform
    {
        public static GridSetting GridSetting;
        
        [HideInInspector] public Transform transform;
        public Vector2 position
        {
            get => transform.position;
            set => transform.position = new Vector3(value.x, value.y, transform.position.z);
        }
        public IntVector2 GridPosition;

        public GridTransform(Transform transform)
        {
            this.transform = transform;
        }

        public void Update()
        {
            GridPosition = new IntVector2(transform.position.x / GridSetting.GridWidth, transform.position.y / GridSetting.GridHeight);
        }

        public void SnapToGrid()
        {
            transform.position = new Vector3(GridPosition.x * GridSetting.GridWidth, GridPosition.y * GridSetting.GridHeight, transform.position.z);
        }

        public static Vector2 Snap(Vector2 position)
        {
            Vector2 offset = position - GridSetting.Origin;

            int snapX = (int)(offset.x / GridSetting.GridWidth);
            int snapY = (int)(offset.y / GridSetting.GridHeight);
            return new Vector2(snapX * GridSetting.GridWidth, snapY * GridSetting.GridHeight);
        }

        public static Vector2 GetGridSize()
        {
            return new Vector2(GridSetting.GridWidth, GridSetting.GridHeight);
        }
        
        public Rect GetGridRect()
        {
            return new Rect(GridPosition.x * GridSetting.GridWidth, GridPosition.y * GridSetting.GridHeight, GridSetting.GridWidth, GridSetting.GridHeight);
        }

        public static Vector2 GetVelocityByGrid(int x, int y, float gravity)
        {
            float realX = x * GridSetting.GridWidth;
            float realY = y * GridSetting.GridHeight;
            
            float yVelocity = Mathf.Sqrt(realY * gravity * 2);
            float xVelocity = realX / (yVelocity / gravity * 2);
            

            Vector2 result = new Vector2(xVelocity, yVelocity);
            
            return result;
        }
    }

    [Serializable]
    public struct IntVector2
    {
        public int x;
        public int y;

        public IntVector2(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public IntVector2(float x, float y)
        {
            this.x = Mathf.RoundToInt(x);
            this.y = Mathf.RoundToInt(y);
        }

        public IntVector2(Vector2 vector2) : this(vector2.x, vector2.y)
        {
        }
    }
}