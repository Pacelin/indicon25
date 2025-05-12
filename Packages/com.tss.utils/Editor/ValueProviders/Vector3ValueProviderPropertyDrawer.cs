using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace TSS.Utils.Editor.ValueProviders
{
    [CustomPropertyDrawer(typeof(IVector3ValueProvider))]
    public class Vector3ValueProviderPropertyDrawer : ValueProviderPropertyDrawer
    {
        protected override object GetDefaultObject() => new Vector3ValueProvider(Vector3.zero);
        
        protected override void DrawObject(Rect rect, SerializedProperty property)
        {
            if (property.managedReferenceValue is Vector3ValueProvider)
            {
                var valueProp = property.FindPropertyRelative("_value");
                EditorGUI.PropertyField(rect, valueProp, GUIContent.none);
            }
            else if (property.managedReferenceValue is RandomOnLineVector3ValueProvider)
            {
                var rects = SplitRows(rect, 2);
                var fromProp = property.FindPropertyRelative("_from");
                var toProp = property.FindPropertyRelative("_to");
                TSSEditorGUI.BeginSmallLabel();
                EditorGUI.PropertyField(rects[0], fromProp, new GUIContent("From"));
                EditorGUI.PropertyField(rects[1], toProp, new GUIContent("To"));
                TSSEditorGUI.EndSmallLabel();
            }
            else if (property.managedReferenceValue is RandomInSphereVector3ValueProvider)
            {
                var rects = SplitRows(rect, 2);
                var posProp = property.FindPropertyRelative("_circlePosition");
                var radiusProp = property.FindPropertyRelative("_circleRadius");
                TSSEditorGUI.BeginSmallLabel();
                EditorGUI.PropertyField(rects[0], posProp, new GUIContent("Position"));
                EditorGUI.PropertyField(rects[1], radiusProp, new GUIContent("Radius"));
                TSSEditorGUI.EndSmallLabel();
            }
            else if (property.managedReferenceValue is Vector3UniformValueProvider)
            {
                var valueProp = property.FindPropertyRelative("_value");
                EditorGUI.PropertyField(rect, valueProp, GUIContent.none);
            }
            else if (property.managedReferenceValue is RandomInBoundsVector3ValueProvider)
            {
                var boundsProp = property.FindPropertyRelative("_bounds");
                EditorGUI.PropertyField(rect, boundsProp, GUIContent.none);
            }
            else if (property.managedReferenceValue is Vector3FollowValueProvider)
            {
                var targetProp = property.FindPropertyRelative("_target");
                EditorGUI.PropertyField(rect, targetProp, GUIContent.none);
            }
        }

        protected override int GetRowsCount(object value)
        {
            if (value is RandomOnLineVector3ValueProvider)
                return 2;
            if (value is RandomInSphereVector3ValueProvider)
                return 2;
            if (value is RandomInBoundsVector3ValueProvider)
                return 2;
            return 1;
        }

        protected override string LabelPostfix(object value)
        {
            if (value is RandomOnLineVector3ValueProvider)
                return " (Random On Line)";
            if (value is RandomInSphereVector3ValueProvider)
                return " (Random In Sphere)";
            if (value is RandomInBoundsVector3ValueProvider)
                return " (Random In Bounds)";
            if (value is Vector3UniformValueProvider)
                return " (Uniform)";
            if (value is Vector3FollowValueProvider)
                return " (Follow)";
            return base.LabelPostfix(value);
        }

        protected override IEnumerable<ValueProviderEntry> GetPossibleEntries()
        {
            yield return ValueProviderEntry.Create<Vector3ValueProvider>("Value", 
                () => new Vector3ValueProvider(Vector3.zero));
            yield return ValueProviderEntry.Create<Vector3UniformValueProvider>("One Scaled",
                () => new Vector3UniformValueProvider(1));
            yield return ValueProviderEntry.Create<RandomOnLineVector3ValueProvider>("Random On Line",
                () => new RandomOnLineVector3ValueProvider(Vector3.zero, Vector3.right));
            yield return ValueProviderEntry.Create<RandomInSphereVector3ValueProvider>("Random In Sphere",
                () => new RandomInSphereVector3ValueProvider(Vector3.zero, 1));
            yield return ValueProviderEntry.Create<RandomInBoundsVector3ValueProvider>("Random In Bounds",
                () => new RandomInBoundsVector3ValueProvider(new Bounds(Vector3.zero, Vector3.one)));
            yield return ValueProviderEntry.Create<Vector3FollowValueProvider>("Follow",
                () => new Vector3FollowValueProvider());
        }
    }
}