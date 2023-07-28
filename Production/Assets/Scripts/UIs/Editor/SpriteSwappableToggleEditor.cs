using MoonBunny.UIs;
using UnityEditor;
using UnityEditor.UI;
using UnityEngine.UIElements;

namespace UIs.Editor
{
    [CustomEditor(typeof(SpriteSwappableToggle), true)]
    public class SpriteSwappableToggleEditor : ToggleEditor
    {
        private SerializedProperty _targetImage;
        private SerializedProperty _onSprite;
        private SerializedProperty _offSprite;
        protected override void OnEnable()
        {
            base.OnEnable();

            _targetImage = serializedObject.FindProperty("SwapTargetImage");
            _onSprite = serializedObject.FindProperty("ToggleOnSprite");
            _offSprite = serializedObject.FindProperty("ToggleOffSprite");
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            
            serializedObject.Update();

            EditorGUILayout.PropertyField(_targetImage);
            EditorGUILayout.PropertyField(_onSprite);
            EditorGUILayout.PropertyField(_offSprite);
            
            serializedObject.ApplyModifiedProperties();
        }
    }
}