using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace TSS.Utils.Editor.ValueProviders
{
    [CustomPropertyDrawer(typeof(IColorValueProvider))]
    public class ColorValueProviderPropertyDrawer : ValueProviderPropertyDrawer
    {
        protected override object GetDefaultObject() => new ColorValueProvider(Color.white);
        
        protected override void DrawObject(Rect rect, SerializedProperty property)
        {
            if (property.managedReferenceValue is ColorValueProvider)
            {
                var prop = property.FindPropertyRelative("_color");
                EditorGUI.PropertyField(rect, prop, GUIContent.none);
            }
            else if (property.managedReferenceValue is RandomColorValueProvider)
            {
                TSSEditorGUI.BeginSmallLabel();
                var prop = property.FindPropertyRelative("_alpha");
                EditorGUI.PropertyField(rect, prop, new GUIContent("Alpha"));
                TSSEditorGUI.EndSmallLabel();
            }
            else if (property.managedReferenceValue is RandomColorSelectedValueProvider)
            {
                var prop = property.FindPropertyRelative("_availableColors");
                EditorGUI.PropertyField(rect, prop, GUIContent.none);
            }
            else if (property.managedReferenceValue is RandomColorGradientValueProvider)
            {
                var prop = property.FindPropertyRelative("_gradient");
                EditorGUI.PropertyField(rect, prop, GUIContent.none);
            }
        }

        protected override int GetRowsCount(object value)
        {
            return 1;
        }

        protected override float GetInsideHeight(SerializedProperty property)
        {
            if (property.managedReferenceValue is RandomColorSelectedValueProvider)
                return EditorGUI.GetPropertyHeight(property.FindPropertyRelative("_availableColors"), true);
            return 0;
        }

        protected override string LabelPostfix(object value)
        {
            if (value is RandomColorValueProvider)
                return " (Random)";
            if (value is RandomColorSelectedValueProvider)
                return " (Random)";
            if (value is RandomColorGradientValueProvider)
                return " (Random)";
            return "";
        }

        protected override IEnumerable<ValueProviderEntry> GetPossibleEntries()
        {
            yield return ValueProviderEntry.Create<ColorValueProvider>("Value",
                () => new ColorValueProvider(Color.white));
            yield return ValueProviderEntry.Create<RandomColorValueProvider>("Random",
                () => new RandomColorValueProvider(1));
            yield return ValueProviderEntry.Create<RandomColorSelectedValueProvider>("Random Selected",
                () => new RandomColorSelectedValueProvider());
            yield return ValueProviderEntry.Create<RandomColorGradientValueProvider>("Random Gradient",
                () => new RandomColorGradientValueProvider());
        }
    }
}