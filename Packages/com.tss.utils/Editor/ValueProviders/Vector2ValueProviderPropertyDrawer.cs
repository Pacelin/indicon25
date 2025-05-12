using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace TSS.Utils.Editor.ValueProviders
{
    [CustomPropertyDrawer(typeof(IVector2ValueProvider))]
    public class Vector2ValueProviderPropertyDrawer : ValueProviderPropertyDrawer
    {
        protected override object GetDefaultObject() => new Vector2ValueProvider(Vector3.zero);
        
        protected override void DrawObject(Rect rect, SerializedProperty property)
        {
            if (property.managedReferenceValue is Vector2ValueProvider)
            {
                var valueProp = property.FindPropertyRelative("_value");
                EditorGUI.PropertyField(rect, valueProp, GUIContent.none);
            }
            else if (property.managedReferenceValue is RandomOnLineVector2ValueProvider)
            {
                var rects = SplitRows(rect, 2);
                var fromProp = property.FindPropertyRelative("_from");
                var toProp = property.FindPropertyRelative("_to");
                TSSEditorGUI.BeginSmallLabel();
                EditorGUI.PropertyField(rects[0], fromProp, new GUIContent("From"));
                EditorGUI.PropertyField(rects[1], toProp, new GUIContent("To"));
                TSSEditorGUI.EndSmallLabel();
            }
            else if (property.managedReferenceValue is RandomInCircleVector2ValueProvider)
            {
                var rects = SplitRows(rect, 2);
                var posProp = property.FindPropertyRelative("_circlePosition");
                var radiusProp = property.FindPropertyRelative("_circleRadius");
                TSSEditorGUI.BeginSmallLabel();
                EditorGUI.PropertyField(rects[0], posProp, new GUIContent("Position"));
                EditorGUI.PropertyField(rects[1], radiusProp, new GUIContent("Radius"));
                TSSEditorGUI.EndSmallLabel();
            }
            else if (property.managedReferenceValue is Vector2OneScaledValueProvider)
            {
                var valueProp = property.FindPropertyRelative("_value");
                EditorGUI.PropertyField(rect, valueProp, GUIContent.none);
            }
            else if (property.managedReferenceValue is RandomInRectVector2ValueProvider)
            {
                var boundsProp = property.FindPropertyRelative("_rect");
                EditorGUI.PropertyField(rect, boundsProp, GUIContent.none);
            }
            else if (property.managedReferenceValue is Vector2FollowValueProvider)
            {
                var targetProp = property.FindPropertyRelative("_target");
                EditorGUI.PropertyField(rect, targetProp, GUIContent.none);
            }
        }

        protected override int GetRowsCount(object value)
        {
            if (value is RandomOnLineVector2ValueProvider)
                return 2;
            if (value is RandomInCircleVector2ValueProvider)
                return 2;
            if (value is RandomInRectVector2ValueProvider)
                return 2;
            return 1;
        }

        protected override string LabelPostfix(object value)
        {
            if (value is RandomOnLineVector2ValueProvider)
                return " (Random On Line)";
            if (value is RandomInCircleVector2ValueProvider)
                return " (Random In Circle)";
            if (value is RandomInRectVector2ValueProvider)
                return " (Random In Rect)";
            if (value is Vector2OneScaledValueProvider)
                return " (One Scaled)";
            if (value is Vector2FollowValueProvider)
                return " (Follow)";
            return base.LabelPostfix(value);
        }

        protected override IEnumerable<ValueProviderEntry> GetPossibleEntries()
        {
            yield return ValueProviderEntry.Create<Vector2ValueProvider>("Value", 
                () => new Vector2ValueProvider(Vector2.zero));
            yield return ValueProviderEntry.Create<Vector2OneScaledValueProvider>("One Scaled",
                () => new Vector2OneScaledValueProvider(1));
            yield return ValueProviderEntry.Create<RandomOnLineVector2ValueProvider>("Random On Line",
                () => new RandomOnLineVector2ValueProvider(Vector2.zero, Vector2.right));
            yield return ValueProviderEntry.Create<RandomInCircleVector2ValueProvider>("Random In Circle",
                () => new RandomInCircleVector2ValueProvider(Vector2.zero, 1));
            yield return ValueProviderEntry.Create<RandomInRectVector2ValueProvider>("Random In Rect",
                () => new RandomInRectVector2ValueProvider(new Rect(Vector2.zero, Vector2.one)));
            yield return ValueProviderEntry.Create<Vector2FollowValueProvider>("Follow",
                () => new Vector2FollowValueProvider());
        }
    }
}