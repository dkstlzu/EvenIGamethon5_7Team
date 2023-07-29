using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace dkstlzu.Utility
{
    public class InheritanceListEditor<T> : PropertyDrawer
    {

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            property.serializedObject.Update();

            
            
            property.serializedObject.ApplyModifiedProperties();
        }

    //     public override VisualElement CreatePropertyGUI(SerializedProperty property)
    //     {
    //         VisualElement root = new VisualElement();
    //
    //         Foldout foldout = new Foldout();
    //         foldout.text = property.displayName;
    //         root.Add(foldout);
    //
    //         SerializedPropertyDebug.LogAllPropertyPath(property);
    //
    //         for (int i = 0; i < property.arraySize; i++)
    //         {
    //             PropertyField elementField = new PropertyField(property.GetArrayElementAtIndex(i));
    //             foldout.Add(elementField);
    //         }
    //
    //         VisualElement buttonBox = new VisualElement();
    //
    //         Button addButton = new Button(() => 
    //         {
    //             property.serializedObject.ApplyModifiedProperties();
    //             property.serializedObject.Update();
    //         });
    //         addButton.text = "ADD";
    //
    //         Button minusButton = new Button(() => 
    //         {
    //
    //             property.serializedObject.ApplyModifiedProperties();
    //             property.serializedObject.Update();
    //         });
    //         minusButton.text = "Delete";
    //
    //         buttonBox.Add(addButton);
    //         buttonBox.Add(minusButton);
    //         buttonBox.style.flexDirection = new StyleEnum<FlexDirection>(FlexDirection.Row);
    //
    //         foldout.Add(buttonBox);
    //
    //         return root;
    //     }
    }

    public class InheritSwapListPropertyDrawer<T> : PropertyDrawer
    {
        // public override VisualElement CreatePropertyGUI(SerializedProperty property)
        // {
        //     VisualElement root = new VisualElement();

        //     if (UnityEditor.EditorApplication.isPlaying)
        //     {
        //         Debug.Log("Playing");
        //         // root.Add(new PropertyField(property.FindPropertyRelative("InheritanceList")));
        //     } else
        //     {
        //         Debug.Log("Not Playing");
        //         return base.CreatePropertyGUI(property);
        //     }

        //     return root;
        // }
    }
}
