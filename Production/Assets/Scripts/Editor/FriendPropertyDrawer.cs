using System;
using System.IO;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace MoonBunny.CustomEditors
{
    [CustomPropertyDrawer(typeof(Friend))]
    public class FriendPropertyDrawer : PropertyDrawer
    {
        private const string SpecPath = "Assets/Resources/Specs/Friend/";
        
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            VisualElement root = new VisualElement();
            
            Foldout foldout = new Foldout();
            foldout.text = "Friend";

            PropertyField JumpPower = new PropertyField(property.FindPropertyRelative("JumpPower"));
            PropertyField HP = new PropertyField(property.FindPropertyRelative("CurrentHp"));

            SerializedProperty nameSP = property.FindPropertyRelative("_name");
            
            EnumField namefield = new EnumField("Type", Enum.Parse<FriendName>(nameSP.enumNames[nameSP.enumValueIndex]));

            namefield.RegisterValueChangedCallback((ce) => OnNameChanged((FriendName)ce.newValue, property));

            foldout.Insert(0, namefield);
            foldout.Add(JumpPower);
            foldout.Add(HP);
            
            root.Add(foldout);
            return root;
        }

        private void OnNameChanged(FriendName newName, SerializedProperty property)
        {
            string finalPath = Path.Combine(SpecPath, newName.ToString() + ".asset");

            FriendSpec friendSpec = AssetDatabase.LoadAssetAtPath<FriendSpec>(finalPath);

            property.FindPropertyRelative("_name").intValue = (int)(newName);
            // property.FindPropertyRelative("JumpPower").intValue = friendSpec.HorizontalJumpSpeed;
            // property.FindPropertyRelative("JumpPower").intValue = friendSpec.VerticalJumpSpeed;
            property.serializedObject.ApplyModifiedProperties();
            property.serializedObject.Update();
        }
    }
}