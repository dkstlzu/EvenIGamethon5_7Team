using System;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace dkstlzu.Utility
{
    public class EnumDictElementPropertyDrawer<T, M> : PropertyDrawer where T : struct, Enum
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            VisualElement root = new VisualElement();
            
            int enumIndex = property.FindPropertyRelative("EnumKey").enumValueIndex;
            string enumStr = property.FindPropertyRelative("EnumKey").enumNames[enumIndex];

            T t;
            if (!Enum.TryParse<T>(enumStr, out t))
            {
                Label errorLabel = new Label("Fail to Parsing Enum Check Again EditorScript");
                root.Add(errorLabel);
            }
            else
            {
                EnumField enumField = new EnumField(t);
                PropertyField valueField = new PropertyField(property.FindPropertyRelative("Value"), "");

                root.style.flexDirection = new StyleEnum<FlexDirection>(FlexDirection.Row);
                enumField.style.flexGrow = 0.3f;
                valueField.style.flexGrow = 1;
                
                root.Add(enumField);
                root.Add(valueField);
            }
            
            return root;
        }
    }

}