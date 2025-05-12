using TSS.Utils.Randoms.Weighted;
using UnityEditor;
using UnityEngine;

namespace TSS.Utils.Editor.Randoms.Weighted
{
    [CustomPropertyDrawer(typeof(RandomWeighted<>))]
    public class RandomWeightedDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.PropertyField(position, property.FindPropertyRelative("_entries"), label);
            property.serializedObject.ApplyModifiedProperties();
            property.serializedObject.Update();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property.FindPropertyRelative("_entries"));
        }
    }
}