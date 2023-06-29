using System;
using UnityEditor;
using UnityEngine;

namespace EvenI7.Proto.CustomEditors
{
    [CustomEditor(typeof(LevelSummoner))]
    public class LevelSummonerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            LevelSummoner summoner = target as LevelSummoner;
            serializedObject.Update();
            base.OnInspectorGUI();

            if (GUILayout.Button("Summon"))
            {
                summoner.Summon();
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}