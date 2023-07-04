using UnityEditor;
using UnityEngine.UIElements;

namespace MoonBunny.CustomEditors
{
    [CustomEditor(typeof(Item))]
    public class ItemEditor : Editor
    {
        public override VisualElement CreateInspectorGUI()
        {
            
            return base.CreateInspectorGUI();
        }
    }
}