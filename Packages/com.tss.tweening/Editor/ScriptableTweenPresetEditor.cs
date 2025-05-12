using UnityEditor;

namespace TSS.Tweening.Editor
{
    [CustomEditor(typeof(ScriptableTweenPreset))]
    public class ScriptableTweenPresetEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            ScriptableTweenItemPropertyDrawer.TargetProvider = serializedObject;
            EditorGUILayout.PropertyField(serializedObject.FindProperty("_items"));
            serializedObject.ApplyModifiedProperties();
            serializedObject.Update();
        }
    }
}