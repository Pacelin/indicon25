using TSS.Utils.Randoms.Weighted;
using UnityEditor;
using UnityEngine;

namespace TSS.Utils.Editor.Randoms.Weighted
{
    [CustomPropertyDrawer(typeof(WeightedEntry<>))]
    public class WeightedEntryDrawer : PropertyDrawer
    {
        private const float HORIZONTAL_SPACING = 4;
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var valueProp = property.FindPropertyRelative("Value");
            var weightProp = property.FindPropertyRelative("Weight");
            
            var elementWidth = position.width / 2 - HORIZONTAL_SPACING;
            var valueRect = new Rect(position.x, position.y, elementWidth, EditorGUI.GetPropertyHeight(valueProp));
            var weightRect = new Rect(position.x + elementWidth + HORIZONTAL_SPACING, position.y, elementWidth,
                EditorGUIUtility.singleLineHeight);

            EditorGUI.PropertyField(valueRect, valueProp, GUIContent.none);
            TSSEditorGUI.BeginSmallLabel();
            EditorGUI.PropertyField(weightRect, weightProp, new GUIContent("Weight: "));
            TSSEditorGUI.EndSmallLabel();
            
            if (weightProp.floatValue < 0)
                weightProp.floatValue = 0;
            property.serializedObject.ApplyModifiedProperties();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property.FindPropertyRelative("Value"));
        }
    }
}