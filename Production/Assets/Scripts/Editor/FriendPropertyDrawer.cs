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

            PropertyField StartJumpHorizontal = new PropertyField(property.FindPropertyRelative("StartJumpHorizontalSpeed"));
            PropertyField StartJumpVertical = new PropertyField(property.FindPropertyRelative("StartJumpVerticalSpeed"));
            PropertyField BouncyPower = new PropertyField(property.FindPropertyRelative("BouncyPower"));
            PropertyField PushingPlatformPower = new PropertyField(property.FindPropertyRelative("PushingPlatformPower"));
            PropertyField MaxHp = new PropertyField(property.FindPropertyRelative("MaxHp"));
            SerializedProperty nameSP = property.FindPropertyRelative("_name");
            
            foldout.Add(StartJumpHorizontal);
            foldout.Add(StartJumpVertical);
            foldout.Add(BouncyPower);
            foldout.Add(PushingPlatformPower);
            foldout.Add(MaxHp);

            EnumField namefield = new EnumField("Type", Enum.Parse<FriendName>(nameSP.enumNames[nameSP.enumValueIndex]));

            namefield.RegisterValueChangedCallback((ce) => OnNameChanged((FriendName)ce.newValue, property));

            foldout.Insert(0, namefield);
            
            root.Add(foldout);
            return root;
        }

        private void OnNameChanged(FriendName newName, SerializedProperty property)
        {
            string finalPath = Path.Combine(SpecPath, newName.ToString() + ".asset");

            FriendSpec friendSpec = AssetDatabase.LoadAssetAtPath<FriendSpec>(finalPath);

            property.FindPropertyRelative("_name").intValue = (int)(newName);
            property.FindPropertyRelative("StartJumpHorizontalSpeed").floatValue = friendSpec.StartJumpHorizontalSpeed;
            property.FindPropertyRelative("StartJumpVerticalSpeed").floatValue = friendSpec.StartJumpHorizontalSpeed;
            property.FindPropertyRelative("BouncyPower").floatValue = friendSpec.BouncyPower;
            property.FindPropertyRelative("MaxHp").intValue = friendSpec.MaxHp;
            property.serializedObject.ApplyModifiedProperties();
            property.serializedObject.Update();
        }
    }
}