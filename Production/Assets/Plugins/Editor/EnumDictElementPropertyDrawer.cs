using System;
using System.Collections.Generic;
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
                PropertyField enumField = new PropertyField(property.FindPropertyRelative("EnumKey"), "");
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

    public class EnumDictPropertyDrawer<T, M> : PropertyDrawer where T : struct, Enum
    {
        private Box _listBox;
        private SerializedObject _serializedObject;
        private string _propertyPath;
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            VisualElement root = new VisualElement();

            _serializedObject = property.serializedObject;

            _propertyPath = property.propertyPath;

            Foldout foldout = new Foldout();
            foldout.text = property.displayName;
            
            root.Add(foldout);

            SerializedProperty listProperty = property.FindPropertyRelative("_elementList");

            _listBox = new Box();
            foldout.Add(_listBox);
            //
            // VisualElement listFooter = new VisualElement();
            // listFooter.style.alignSelf = new StyleEnum<Align>(Align.FlexEnd);

            // Button plusButton = new Button(OnPlusButtonClicked);
            // plusButton.text = "+";
            // plusButton.AddToClassList("unity-text-element");
            // plusButton.AddToClassList("unity-button");
            //
            // Button minusButton = new Button(OnMinusButtonClicked);
            // minusButton.text = "-";
            // minusButton.AddToClassList("unity-text-element");
            // minusButton.AddToClassList("unity-button");

            // listFooter.style.flexDirection = new StyleEnum<FlexDirection>(FlexDirection.Row);
            // listFooter.Add(plusButton);
            // listFooter.Add(minusButton);
            //
            // foldout.Add(listFooter);

            for (int i = 0; i < listProperty.arraySize; i++)
            {
                PropertyField elementField = new PropertyField(listProperty.GetArrayElementAtIndex(i));
                _listBox.Add(elementField);
            }

            return root;
        }

        void OnPlusButtonClicked()
        {
            SerializedProperty serializedProperty = _serializedObject.FindProperty(_propertyPath + "._elementList");

            SerializedPropertyDebug.LogAllPropertyPath(_serializedObject);
            Debug.Log($"SSS {serializedProperty.propertyPath}");
            
            Debug.Log($"WTF1 {serializedProperty.arraySize}");
            serializedProperty.InsertArrayElementAtIndex(serializedProperty.arraySize);
            _listBox.Add(new PropertyField(serializedProperty.GetArrayElementAtIndex(serializedProperty.arraySize-1)));

            _serializedObject.ApplyModifiedProperties();
            _serializedObject.Update();
        }

        void OnMinusButtonClicked()
        {
            SerializedProperty serializedProperty = _serializedObject.FindProperty(_propertyPath + "._elementList");

            Debug.Log($"WTF2 {serializedProperty.arraySize}");
            if (serializedProperty.arraySize > 0)
            {
                _listBox.RemoveAt(serializedProperty.arraySize - 1);

                serializedProperty.DeleteArrayElementAtIndex(serializedProperty.arraySize-1);
                Debug.Log($"WTF3 {serializedProperty.arraySize}");

                _serializedObject.ApplyModifiedProperties();
                _serializedObject.Update();
            }
        }
    }

    public class ReadOnlyEnumDictPropertyDrawer<T, M> : EnumDictPropertyDrawer<T, M> where T : struct, Enum
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            VisualElement root = base.CreatePropertyGUI(property);

            root.Q<Box>().SetEnabled(false);
            
            return root;
        }
    }
}