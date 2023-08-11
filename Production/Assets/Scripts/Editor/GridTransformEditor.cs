using MoonBunny;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace MoonBunny.CustomEditors
{
    [CustomPropertyDrawer(typeof(GridTransform))]
    public class GridTransformEditor : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            VisualElement root = new VisualElement();

            PropertyField gridPosition = new PropertyField(property.FindPropertyRelative("GridPosition"));
            PropertyField snapMethod = new PropertyField(property.FindPropertyRelative("SnapMethod"));
            root.Add(gridPosition);
            root.Add(snapMethod);

            return root;
        }
    }
}