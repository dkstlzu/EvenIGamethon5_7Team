﻿using System;
using System.IO;
using dkstlzu.Utility;
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
        private const string Space = "   ";

        private VisualElement root;
        
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            root = new VisualElement();
            
            Foldout foldout = new Foldout();
            foldout.text = "Friend";

            PropertyField spec = new PropertyField(property.FindPropertyRelative("_spec"));
            Label nameSP = new Label("Name" + Space + property.FindPropertyRelative("_name").enumNames[property.FindPropertyRelative("_name").enumValueIndex]);
            Label jumpPower = new Label("JumpPower" + Space + property.FindPropertyRelative("JumpPower.x").intValue + Space + property.FindPropertyRelative("JumpPower.y").intValue);
            Label magneticPower = new Label("MagneticPower" + Space + property.FindPropertyRelative("MagneticPower").intValue);

            spec.RegisterValueChangeCallback((evt) => OnSpecChanged((FriendSpec)evt.changedProperty.objectReferenceValue, property));

            foldout.Add(spec);
            foldout.Add(nameSP);
            foldout.Add(jumpPower);
            foldout.Add(magneticPower);
            
            root.Add(foldout);
            return root;
        }

        private void OnSpecChanged(FriendSpec changedSpec, SerializedProperty property)
        {
            if (changedSpec == null) return;
            
            property.FindPropertyRelative("_name").intValue = (int)(changedSpec.Name);
            property.FindPropertyRelative("JumpPower.x").intValue = changedSpec.HorizontalJumpSpeed;
            property.FindPropertyRelative("JumpPower.y").intValue = changedSpec.VerticalJumpSpeed;
            property.FindPropertyRelative("MagneticPower").intValue = changedSpec.MagneticPower;
            
            property.serializedObject.ApplyModifiedProperties();
            property.serializedObject.Update();
        }
    }
}