using System;
using UnityEditor;
using UnityEngine;

namespace MoonBunny.CustomEditors
{
    [CustomEditor(typeof(GameManager))]
    public class GameManagerEditor : Editor
    {
        private SerializedProperty GlobalProperty;

        private void OnEnable()
        {
            GlobalProperty = serializedObject.FindProperty("GlobalGravity");
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            
            serializedObject.Update();

            EditorGUILayout.Slider(GlobalProperty, 0, 20);
            Physics2D.gravity = new Vector2(0, -GlobalProperty.floatValue);
            
            serializedObject.ApplyModifiedProperties();
        }
    }
}