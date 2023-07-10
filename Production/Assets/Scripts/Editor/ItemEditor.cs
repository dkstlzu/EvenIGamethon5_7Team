using System;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

namespace MoonBunny.CustomEditors
{
    [CustomEditor(typeof(Item))]
    public class ItemEditor : Editor
    {
        private const string SpecPath = "Assets/Resources/Specs/Item/";
        
        public override VisualElement CreateInspectorGUI()
        {
            VisualElement root = new VisualElement();

            PropertyField ScoreField = new PropertyField(serializedObject.FindProperty("Score"));
            PropertyField AudioSource = new PropertyField(serializedObject.FindProperty("_audioSource"));
            PropertyField Renderer = new PropertyField(serializedObject.FindProperty("_renderer"));
            SerializedProperty typeSP = serializedObject.FindProperty("_type");
            
            root.Add(ScoreField);
            root.Add(AudioSource);
            root.Add(Renderer);

            EnumField typeField = new EnumField("Type", Enum.Parse<ItemType>(typeSP.enumNames[typeSP.enumValueIndex]));

            typeField.RegisterValueChangedCallback((ce) => OnTypeChanged((ItemType)ce.newValue));
            
            root.Insert(0, typeField);
            
            return root;
        }

        private void OnTypeChanged(ItemType newType)
        {
            string finalPath = System.IO.Path.Combine(SpecPath, newType.ToString() + ".asset");

            ItemSpec itemSpec = AssetDatabase.LoadAssetAtPath<ItemSpec>(finalPath);
            
            serializedObject.FindProperty("_type").intValue = (int)newType;
            serializedObject.FindProperty("Score").intValue = itemSpec.Score;
            serializedObject.FindProperty("_audioSource").objectReferenceValue = itemSpec.AudioClip;

            Object spriteRenderer = serializedObject.FindProperty("_renderer").objectReferenceValue;

            if (spriteRenderer == null)
            {
                Debug.LogWarning("Check the SpriteRenderer reference in inspector. modification of sprite is not applied correctly");    
            }
            else
            {
                SerializedObject spriteRendererSO = new SerializedObject(spriteRenderer);
                spriteRendererSO.FindProperty("m_Sprite").objectReferenceValue = itemSpec.Sprite;
                spriteRendererSO.ApplyModifiedProperties();
                spriteRendererSO.Update();
            }
            
            serializedObject.ApplyModifiedProperties();
            serializedObject.Update();
            
            
        }
    }
}