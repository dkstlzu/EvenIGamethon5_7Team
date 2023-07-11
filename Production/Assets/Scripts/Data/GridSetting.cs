using UnityEngine;

namespace MoonBunny
{
    [CreateAssetMenu(fileName = "GridSetting", menuName = "Settings/GridSetting", order = 0)]
    public class GridSetting : ScriptableObject
    {
        public float CameraOrthogonalSize;
        public float CameraAspect;

        public Vector2 Origin;
        
        public int HorizontalDivision;
        public float GridHeight;
        public float GridWidth;

        [ContextMenu("SetGridWidth")]
        public void SetGridWidth()
        {
            GridWidth = CameraOrthogonalSize * 2 * CameraAspect / HorizontalDivision;
        }
    }
}