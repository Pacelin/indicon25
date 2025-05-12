using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace TSS.Tweening.Editor
{
    [CustomPropertyDrawer(typeof(IScriptableTweenItem), true)]
    public class ScriptableTweenItemPropertyDrawer : PropertyDrawer
    {
        public static SerializedObject TargetProvider { get; set; }
        
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            bool onlyTargetEdit = false;
            var presetProperty = TargetProvider.FindProperty("_preset");
            if (presetProperty != null)
                onlyTargetEdit = (bool) presetProperty.objectReferenceValue;
            
            InitNullObject(property);
            ScriptableTweenItemGUI.DrawHeader(ref position, TargetProvider, property, onlyTargetEdit);
            ScriptableTweenItemGUI.DrawContent(ref position, property);
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            InitNullObject(property);
            return ScriptableTweenItemGUI.GetHeaderHeight(ScriptableTweenItemGUI.GetTargetFor(TargetProvider, property, out _)) +
                   ScriptableTweenItemGUI.GetContentHeight(property);
        }

        private void InitNullObject(SerializedProperty property)
        {
            if (property.managedReferenceValue == null)
            {
                property.managedReferenceValue = new EmptyScriptableTweenItem();
                property.serializedObject.ApplyModifiedProperties();
                property.serializedObject.Update();
            }
        }
    }
}