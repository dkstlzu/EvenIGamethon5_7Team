using UnityEditor;
using UnityEditor.UI;
using UnityEngine.UIElements;

namespace MoonBunny.UIs
{
    [CustomEditor(typeof(ButtonWithSound))]
    public class ButtonWithSoundEditor : ButtonEditor
    {
        private SerializedProperty audioProperty;
        protected override void OnEnable()
        {
            base.OnEnable();

            audioProperty = serializedObject.FindProperty("ButtonSound");
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            
            serializedObject.Update();

            EditorGUILayout.PropertyField(audioProperty);

            serializedObject.ApplyModifiedProperties();
        }
    }
}