using System;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace MoonBunny.CustomEditors
{
    [CustomPropertyDrawer(typeof(Friend))]
    public class FriendPropertyDrawer : PropertyDrawer
    {
        private const string SpecPath = "Assets/Resources/Specs/Friend/";
        private const string Space = "   ";

        private VisualElement root;
        
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            root = new VisualElement();
            
            Foldout foldout = new Foldout();
            foldout.text = "Friend";

            PropertyField spec = new PropertyField(property.FindPropertyRelative("_spec"));
            Label horizontalSpeed = new Label("Speed" + Space + property.FindPropertyRelative("HorizontalSpeed").intValue);
            Label jumpPower = new Label("JumpPower" + Space + property.FindPropertyRelative("JumpPower").floatValue);
            Label magneticPower = new Label("MagneticPower" + Space + property.FindPropertyRelative("MagneticPower").floatValue);

            spec.RegisterValueChangeCallback((evt) => OnSpecChanged((FriendSpec)evt.changedProperty.objectReferenceValue, property));

            foldout.Add(spec);
            foldout.Add(horizontalSpeed);
            foldout.Add(jumpPower);
            foldout.Add(magneticPower);
            
            root.Add(foldout);
            return root;
        }

        private void OnSpecChanged(FriendSpec changedSpec, SerializedProperty property)
        {
            if (changedSpec == null) return;

            FriendName name;
            if (Enum.TryParse(changedSpec.name, out name))
            {
                property.FindPropertyRelative("Name").intValue = (int)name;
            }
            property.FindPropertyRelative("HorizontalSpeed").intValue = changedSpec.HorizontalJumpSpeed;
            property.FindPropertyRelative("JumpPower").floatValue = changedSpec.VerticalJumpSpeed;
            property.FindPropertyRelative("MagneticPower").floatValue = changedSpec.MagneticPower;
            
            property.serializedObject.ApplyModifiedProperties();
            property.serializedObject.Update();
        }
    }
}