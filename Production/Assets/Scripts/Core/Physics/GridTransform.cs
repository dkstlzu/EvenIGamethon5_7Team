using System;
using MoonBunny.Dev;
using UnityEngine;

namespace MoonBunny
{
    [Serializable]
    public class GridTransform
    {
        public static GridSetting GridSetting;
        public static int GridXMin => -(GridSetting.HorizontalDivision-1) / 2;
        public static int GridXMax => GridSetting.HorizontalDivision / 2;
        public static int GridYMin = 0;
        public static Vector2 Origin => GridSetting.Origin;
        public static Vector2 OriginInReal => new Vector2(Origin.x * GridSetting.GridWidth, Origin.y * GridSetting.GridHeight);
        
        public Transform transform;
        public Vector2 position
        {
            get => transform.position;
            set => transform.position = new Vector3(value.x, value.y, transform.position.z);
        }
        public Vector2Int GridPosition;

        public GridTransform(Transform transform)
        {
            this.transform = transform;
        }

        public void Update()
        {
            GridPosition = ToGrid(transform.position);
        }

        public void SnapToGrid()
        {
            transform.position = Snap(transform.position);
        }

        public static Vector2 Snap(Vector2 position)
        {
            Vector2 offset = position - OriginInReal;

            int snapX = Mathf.RoundToInt(offset.x / GridSetting.GridWidth);
            int snapY = Mathf.RoundToInt(offset.y / GridSetting.GridHeight);
            return new Vector2(snapX * GridSetting.GridWidth + OriginInReal.x, snapY * GridSetting.GridHeight + OriginInReal.y);
        }

        public static Vector2 ToReal(Vector2Int gridPosition)
        {
            Vector2 real = OriginInReal;

            real += new Vector2(gridPosition.x * GridSetting.GridWidth, gridPosition.y * GridSetting.GridHeight);
            return real;
        }

        public static Vector2Int ToGrid(Vector2 position)
        {
            Vector2 offset = position - OriginInReal;

            return new Vector2Int(Mathf.RoundToInt(offset.x / GridSetting.GridWidth), Mathf.RoundToInt(offset.y / GridSetting.GridHeight));
        }

        public static bool HasGridObject(Vector2Int gridPosition, out GridObject gridObject)
        {
            Collider2D collider = Physics2D.OverlapBox(ToReal(gridPosition), GetGridSize(), 0);
            if (collider != null)
            {
                collider.TryGetComponent<GridObject>(out gridObject);
                return true;
            }

            gridObject = null;
            return false;
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
}