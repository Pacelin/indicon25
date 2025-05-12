using TSS.Core.UI;
using UnityEditor;
using UnityEngine;

namespace TSS.Core.Editor.UI
{
    [CustomEditor(typeof(TSSProgressBar))]
    public class TSSProgressBarEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            GUI.enabled = false;
            EditorGUILayout.PropertyField(serializedObject.FindProperty("m_Script"));
            GUI.enabled = true;
            
            serializedObject.Update();
            
            EditorGUILayout.PropertyField(serializedObject.FindProperty("_fillMain"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("_fillTrailIncrease"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("_fillTrailDecrease"));
            
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(serializedObject.FindProperty("_minValue"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("_maxValue"));
            EditorGUILayout.EndHorizontal();

            float currentValue = serializedObject.FindProperty("_currentValue").floatValue;
            float currentTrailValue = serializedObject.FindProperty("_currentTrailValue").floatValue;
            float minValue = serializedObject.FindProperty("_minValue").floatValue;
            float maxValue = serializedObject.FindProperty("_maxValue").floatValue;
            
            EditorGUI.BeginChangeCheck();
            currentValue = EditorGUILayout.Slider("Current Value", currentValue, minValue, maxValue);
            if (EditorGUI.EndChangeCheck())
                serializedObject.FindProperty("_currentValue").floatValue = currentValue;

            GUI.enabled = false;
            EditorGUILayout.Slider("Trail Value", currentTrailValue, minValue, maxValue);
            GUI.enabled = true;
            
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(serializedObject.FindProperty("_trailDelay"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("_trailTime"));
            EditorGUILayout.EndHorizontal();
            
            serializedObject.ApplyModifiedProperties();
        }
    }
}