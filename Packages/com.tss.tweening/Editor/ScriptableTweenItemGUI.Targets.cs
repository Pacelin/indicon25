using System;
using UnityEditor;

namespace TSS.Tweening.Editor
{
    public static partial class ScriptableTweenItemGUI
    {
        public static SerializedProperty GetTargetFor(SerializedObject targetProvider, SerializedProperty property, out Type targetType)
        {
            if (property.managedReferenceValue is IScriptableTweenItemNoTarget)
            {
                targetType = null;
                return null;
            }

            var baseType = typeof(IScriptableTweenItem<>);
            var propType = property.managedReferenceValue.GetType();
            targetType = propType.GetInterface(baseType.Name).GetGenericArguments()[0];
            
            var targets = targetProvider.FindProperty("_targets");
            if (targets == null)
                return null;
            
            var items = property.serializedObject.FindProperty("_items");
            if (items == null)
                return property.serializedObject.FindProperty("_target");

            UpdateTargets(targetProvider, property);
            int targetIndex = 0;
            for (int i = 0; i < items.arraySize; i++)
            {
                var item = items.GetArrayElementAtIndex(i);
                if (item.managedReferenceValue is IScriptableTweenItemNoTarget)
                    continue;

                if (item.propertyPath.Equals(property.propertyPath))
                    return targets.GetArrayElementAtIndex(targetIndex);

                targetIndex++;
            }

            return null;
        }
        
        private static void UpdateTargets(SerializedObject targetProvider, SerializedProperty property)
        {
            var targets = targetProvider.FindProperty("_targets");
            if (targets == null)
                return;
            
            var items = property.serializedObject.FindProperty("_items");
            if (items == null)
                return;
            
            int targetsCount = 0;
            for (int i = 0; i < items.arraySize; i++)
            {
                var item = items.GetArrayElementAtIndex(i);
                if (item.managedReferenceValue is IScriptableTweenItemNoTarget)
                    continue;

                targetsCount++;
            }

            if (targets.arraySize != targetsCount)
            {
                targets.arraySize = targetsCount;
                targets.serializedObject.ApplyModifiedProperties();
                targets.serializedObject.Update();
            }
        }
    }
}
