using System;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

namespace MoonBunny.CustomEditors
{
    [CustomEditor(typeof(Obstacle), true)]
    public class ObstacleEditor : GridObjectEditor
    {
        private const string SpecPath = "Assets/Resources/Specs/Obstacle/";

        public override VisualElement CreateInspectorGUI()
        {
            VisualElement root = base.CreateInspectorGUI();

            SerializedProperty typeSP = serializedObject.FindProperty("_type");
                      
            EnumField TypeField = new EnumField("Type", Enum.Parse<ObstacleType>(typeSP.enumNames[typeSP.enumValueIndex]));

            TypeField.RegisterValueChangedCallback((ce) => OnTypeChanged((ObstacleType)ce.newValue));
  
            root.Insert(2, TypeField);
            
            return root;
        }

        private void OnTypeChanged(ObstacleType newType)
        {
            string finalPath = System.IO.Path.Combine(SpecPath, newType.ToString() + ".asset");

            ObstacleSpec obstacleSpec = AssetDatabase.LoadAssetAtPath<ObstacleSpec>(finalPath);

            serializedObject.FindProperty("_type").intValue = (int)newType;
            serializedObject.FindProperty("Damage").intValue = obstacleSpec.Damage;
            serializedObject.FindProperty("_audioSource").objectReferenceValue = obstacleSpec.AudioClip;
            
            Object spriteRenderer = serializedObject.FindProperty("_renderer").objectReferenceValue;

            if (spriteRenderer == null)
            {
                Debug.LogWarning("Check the SpriteRenderer reference in inspector. modification of sprite is not applied correctly");    
            }
            else
            {
                SerializedObject spriteRendererSO = new SerializedObject(spriteRenderer);
                spriteRendererSO.FindProperty("m_Sprite").objectReferenceValue = obstacleSpec.Sprite;
                spriteRendererSO.ApplyModifiedProperties();
                spriteRendererSO.Update();
            }

            serializedObject.ApplyModifiedProperties();
            serializedObject.Update();
        }
    }
}