using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace MoonBunny.CustomEditors
{
    [CustomEditor(typeof(GridObject), true)]
    public class GridObjectEditor : Editor
    {
        public override VisualElement CreateInspectorGUI()
        {
            VisualElement root = new VisualElement();

            PropertyField gridTransform = new PropertyField(serializedObject.FindProperty("GridTransform").FindPropertyRelative("GridPosition"));

            InspectorElement.FillDefaultInspector(root, serializedObject, this);

            root.Insert(1, gridTransform);
            
            return root;
        }
    }
}