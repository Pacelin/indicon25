using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace TSS.Utils.Editor.ValueProviders
{
    [CustomPropertyDrawer(typeof(IFloatValueProvider))]
    public class FloatValueProviderPropertyDrawer : ValueProviderPropertyDrawer
    {
        protected override object GetDefaultObject() => new FloatValueProvider(1);

        protected override void DrawObject(Rect rect, SerializedProperty property)
        {
            if (property.managedReferenceValue is FloatValueProvider)
            {
                var valueProp = property.FindPropertyRelative("_value");
                EditorGUI.PropertyField(rect, valueProp, GUIContent.none);
            } 
            else if (property.managedReferenceValue is FloatRandomBetweenValueProvider)
            {
                var rects = SplitColumns(rect, 2);
                var minProp = property.FindPropertyRelative("_min");
                var maxProp = property.FindPropertyRelative("_max");
                EditorGUI.PropertyField(rects[0], minProp, GUIContent.none);
                EditorGUI.PropertyField(rects[1], maxProp, GUIContent.none);
            }
        }

        protected override int GetRowsCount(object value) => 1;

        protected override IEnumerable<ValueProviderEntry> GetPossibleEntries()
        {
            yield return ValueProviderEntry.Create<FloatValueProvider>("Value", 
                () => new FloatValueProvider(1));
            yield return ValueProviderEntry.Create<FloatRandomBetweenValueProvider>("Random Between",
                () => new FloatRandomBetweenValueProvider(0, 1));
        }
    }
}