using TSS.Utils.Randoms.ADHOC;
using UnityEditor;
using UnityEngine;

namespace TSS.Utils.Editor.Randoms.ADHOC
{
    [CustomPropertyDrawer(typeof(ADHOCEntry<>))]
    public class ADHOCEntryDrawer : PropertyDrawer
    {
        private const float HORIZONTAL_SPACING = 4;
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var valueProp = property.FindPropertyRelative("Value");
            var weightProp = property.FindPropertyRelative("Weight");
            var weightIncreaseOnFailProp = property.FindPropertyRelative("WeightIncreaseOnFail");
            
            var elementWidth = position.width / 3 - HORIZONTAL_SPACING * 2;
            var valueRect = new Rect(position.x, position.y, elementWidth, EditorGUI.GetPropertyHeight(valueProp));
            var weightRect = new Rect(position.x + elementWidth + HORIZONTAL_SPACING, position.y, elementWidth,
                EditorGUIUtility.singleLineHeight);
            var weightIncreaseOnFailRect = new Rect(position.x + elementWidth * 2 + HORIZONTAL_SPACING * 2, position.y,
                elementWidth, EditorGUIUtility.singleLineHeight);

            EditorGUI.PropertyField(valueRect, valueProp, GUIContent.none);
            TSSEditorGUI.BeginSmallLabel();
            EditorGUI.PropertyField(weightRect, weightProp, new GUIContent("Weight: "));
            EditorGUI.PropertyField(weightIncreaseOnFailRect, weightIncreaseOnFailProp, new GUIContent("Adhoc:"));
            TSSEditorGUI.EndSmallLabel();
            
            if (weightProp.floatValue < 0)
                weightProp.floatValue = 0;
            if (weightIncreaseOnFailProp.floatValue < 0)
                weightIncreaseOnFailProp.floatValue = 0;
            property.serializedObject.ApplyModifiedProperties();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property.FindPropertyRelative("Value"));
        }
    }
}