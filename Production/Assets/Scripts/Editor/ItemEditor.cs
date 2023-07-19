using System;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

namespace MoonBunny.CustomEditors
{
    [CustomEditor(typeof(Item), true)]
    public class ItemEditor : GridObjectEditor
    {
        private const string SpecPath = "Assets/Resources/Specs/Item/";
        
        public override VisualElement CreateInspectorGUI()
        {
            VisualElement root = base.CreateInspectorGUI();

            SerializedProperty typeSP = serializedObject.FindProperty("_type");
            
            EnumField typeField = new EnumField("Type", Enum.Parse<ItemType>(typeSP.enumNames[typeSP.enumValueIndex]));

            typeField.RegisterValueChangedCallback((ce) => OnTypeChanged((ItemType)ce.newValue));
            
            root.Insert(2, typeField);
            
            return root;
        }

        private void OnTypeChanged(ItemType newType)
        {
            string finalPath = System.IO.Path.Combine(SpecPath, newType.ToString() + ".asset");

            ItemSpec itemSpec = AssetDatabase.LoadAssetAtPath<ItemSpec>(finalPath);
            
            serializedObject.FindProperty("_type").intValue = (int)newType;
            serializedObject.FindProperty("Score").intValue = itemSpec.Score;
            serializedObject.FindProperty("_audioClip").objectReferenceValue = itemSpec.AudioClip;

            Object spriteRenderer = serializedObject.FindProperty("_renderer").objectReferenceValue;

            if (spriteRenderer == null)
            {
                Debug.LogWarning("Check the SpriteRenderer reference in inspector. modification of sprite is not applied correctly");
                Item item = (Item)serializedObject.targetObject;
                spriteRenderer = item.GetComponentInChildren<SpriteRenderer>();
                serializedObject.FindProperty("_renderer").objectReferenceValue = spriteRenderer;
            }

            if (!spriteRenderer)
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