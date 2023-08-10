using UnityEditor;
using UnityEditor.UI;
using UnityEngine.UIElements;

namespace MoonBunny.UIs
{
    [CustomEditor(typeof(ButtonWithSound))]
    public class ButtonWithSoundEditor : ButtonEditor
    {
        private SerializedProperty audioProperty;
        private SerializedProperty coolTime;
        
        protected override void OnEnable()
        {
            base.OnEnable();

            audioProperty = serializedObject.FindProperty("ButtonSound");
            coolTime = serializedObject.FindProperty("CoolTime");
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            
            serializedObject.Update();

            EditorGUILayout.PropertyField(audioProperty);
            EditorGUILayout.PropertyField(coolTime);

            serializedObject.ApplyModifiedProperties();
        }
    }
}