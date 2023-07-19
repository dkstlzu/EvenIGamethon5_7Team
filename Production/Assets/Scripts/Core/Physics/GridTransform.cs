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

        public static bool HasGridObject(Vector2Int gridPosition)
        {
            GridObject gridObject;
            return HasGridObject(gridPosition, out gridObject);
        }

        private static LayerMask S_findingLayerMask = LayerMask.GetMask(new string[]{
            "Friend","Item","Obstacle","Platform","Character","Gimmick","Ricecake"});

        public static bool HasGridObject(Vector2Int gridPosition, out GridObject gridObject)
        {
            Collider2D[] colliders = Physics2D.OverlapBoxAll(ToReal(gridPosition), GetGridSize(), 0,S_findingLayerMask);
            if (colliders != null && colliders.Length > 0)
            {
                if (colliders[0].TryGetComponent<GridObject>(out gridObject))
                {
                    return true;
                }
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
            if (y <= 0)
            {
                Debug.LogWarning($"GetVelocityByGrid wrong usage. y must be positive");
                return Vector2.zero;
            }

            if (gravity <= 0)
            {
                Debug.LogWarning($"GetVelocityByGrid wrong usage. gravity must be positive");
                return Vector2.zero;
            }
            
            float xDistance = x * GridSetting.GridWidth;
            float yDistance = y * GridSetting.GridHeight;
            
            float yVelocity = Mathf.Sqrt(yDistance * gravity * 2);
            
            float peekTime = yVelocity / gravity;
            float xVelocity = xDistance / (peekTime * 2);
            
            Vector2 result = new Vector2(xVelocity, yVelocity);
            
            return result;
        }

        public static Vector2 GetVelocityByGrid(Vector2Int velocity, float gravity)
        {
            return GetVelocityByGrid(velocity.x, velocity.y, gravity);
        }

        public static float GetVelocityByRelativeHeight(float yVelocity, float gravity, float heightRatio)
        {
            if (yVelocity <= 0)
            {
                Debug.LogWarning($"GetVelocityByRelativeHeight wrong usage. y must be positive");
                return 0;
            }
            
            float peekTime = yVelocity / gravity;

            float peekHeight = yVelocity * peekTime / 2;

            return peekHeight * heightRatio;
        }

        public static Vector2Int GetGridByVelocity(float x, float y, float gravity)
        {
            if (y <= 0)
            {
                Debug.LogWarning($"GetGridByVelocity wrong usage. y must be positive");
                return Vector2Int.zero;
            }

            if (gravity <= 0)
            {
                Debug.LogWarning($"GetGridByVelocity wrong usage. gravity must be positive");
                return Vector2Int.zero;
            }
            
            float peekTime = y / gravity;

            float xDistance = x * peekTime * 2;
            float yDistance = y * peekTime / 2;

            Vector2Int xGrid = ToGrid(new Vector2(xDistance, 0) + OriginInReal);
            Vector2Int yGrid = ToGrid(new Vector2(0, yDistance) + OriginInReal);

            return new Vector2Int(xGrid.x, yGrid.y);
        }

        public static Vector2Int GetGridByVelocity(Vector2 velocity, float gravity)
        {
            return GetGridByVelocity(velocity.x, velocity.y, gravity);
        }

        public static Vector2 ConvertVelocityByGravity(float xVelocity, float yVelocity, float previousGravity, float changingGravity)
        {
            float peekTime = Mathf.Abs(yVelocity / previousGravity);

            float xDistance = xVelocity * peekTime * 2;
            float yDistance = yVelocity * peekTime / 2;
            
            float changedYVelocity = Mathf.Sqrt(yDistance * changingGravity * 2);
            float changedPeekTime = changedYVelocity / changingGravity;
            float changedXVelocity = xDistance / (changedPeekTime * 2);
            
            return new Vector2(changedXVelocity, changedYVelocity);
        }
        
        public static Vector2 ConvertVelocityByGravity(Vector2 velocity, float previousGravity, float changingGravity)
        {
            return ConvertVelocityByGravity(velocity.x, velocity.y, previousGravity, changingGravity);
        }
    }
}