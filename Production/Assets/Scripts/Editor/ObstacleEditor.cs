using UnityEditor;
using UnityEngine.UIElements;

namespace MoonBunny.CustomEditors
{
    [CustomEditor(typeof(Obstacle))]
    public class ObstacleEditor : Editor
    {
        public override VisualElement CreateInspectorGUI()
        {
            
            return base.CreateInspectorGUI();
        }
    }
}