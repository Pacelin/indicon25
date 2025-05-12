using TSS.Utils.Randoms.ADHOC;
using UnityEditor;
using UnityEngine;

namespace TSS.Utils.Editor.Randoms.ADHOC
{
    [CustomPropertyDrawer(typeof(RandomADHOC<>))]
    public class RandomADHOCDrawer : PropertyDrawer
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