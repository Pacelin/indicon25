using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TSS.Utils.Editor;
using UnityEditor;
using UnityEngine;

namespace TSS.Tweening.Editor
{
    public static partial class ScriptableTweenItemGUI
    {
        public readonly struct ItemEntry
        {
            public readonly Type Type;
            public readonly string Path;

            public ItemEntry(Type type, string path)
            {
                Type = type;
                Path = path;
            }
        }

        private static readonly Dictionary<string, bool> _foldoutStates = new();
        private const int CONNECT_BEHAVIOUR_WIDTH = 80;
        private const int HORIZONTAL_SPACING = 4;
        private const int FOLDOUT_WIDTH = 0;
        public const int BOX_PADDING = 4;
        private const int HEADER_LEFT = 4;

        public static void DrawHeader(ref Rect propertyRect, SerializedObject targetProvider, SerializedProperty property, bool editTarget)
        {
            var target = GetTargetFor(targetProvider, property, out var targetType);
            var headerHeight = GetHeaderHeight(target);
            var headerRect = new Rect(propertyRect.x + HEADER_LEFT, propertyRect.y, 
                propertyRect.width - HEADER_LEFT, headerHeight);
            var foldoutRect = new Rect(propertyRect.x + 2, propertyRect.y, 
                FOLDOUT_WIDTH, EditorGUIUtility.singleLineHeight);
            
            BeginDrawBox(ref headerRect, ref propertyRect);

            bool lastGUIState = GUI.enabled;
            if (property.managedReferenceValue.GetType().GetCustomAttribute<NoFoldoutAttribute>() == null)
            {
                GUI.enabled = true;
                if (EditorGUI.Foldout(foldoutRect, GetFoldoutState(property), GUIContent.none) != GetFoldoutState(property))
                    SetFoldoutState(property, !GetFoldoutState(property));
                GUI.enabled = lastGUIState;
            }
            else if (GetFoldoutState(property))
            {
                SetFoldoutState(property, false);
            }

            Rect buttonRect = new Rect(headerRect.x, headerRect.y, 
                headerRect.width, EditorGUIUtility.singleLineHeight);
            propertyRect.y += EditorGUIUtility.singleLineHeight;
            headerRect.y += EditorGUIUtility.singleLineHeight;
            
            var connectBehaviourProperty = property.FindPropertyRelative("_connectBehaviour");
            if (connectBehaviourProperty != null)
            {
                EditorGUI.PropertyField(new Rect(buttonRect.x, buttonRect.y, CONNECT_BEHAVIOUR_WIDTH, EditorGUIUtility.singleLineHeight),
                    connectBehaviourProperty, GUIContent.none);
                buttonRect.x += CONNECT_BEHAVIOUR_WIDTH + HORIZONTAL_SPACING;
                buttonRect.width -= CONNECT_BEHAVIOUR_WIDTH + HORIZONTAL_SPACING;
            }
            
            var items = GetItems(property.managedReferenceValue.GetType(), out string currentPath);
            var filterNotPresets = targetProvider.targetObject is ScriptableTweenPreset;
            if (EditorGUI.DropdownButton(buttonRect, new GUIContent(currentPath), FocusType.Passive))
            {
                var menu = new GenericMenu();
                foreach (var item in items)
                {
                    if (filterNotPresets && item.Type.GetCustomAttribute<NotPresetAttribute>() != null)
                        continue;
                    menu.AddItem(new GUIContent(item.Path), false, () =>
                    {
                        if (property.managedReferenceValue.GetType() == item.Type)
                            return;
                        property.managedReferenceValue = Activator.CreateInstance(item.Type);
                        property.serializedObject.ApplyModifiedProperties();
                        property.serializedObject.Update();
                    });
                }
                menu.ShowAsContext();
            }
            
            if (target != null)
            {
                GUI.enabled = true;
                var targetFieldRect = new Rect(headerRect.x, headerRect.y + EditorGUIUtility.standardVerticalSpacing,
                    headerRect.width, EditorGUIUtility.singleLineHeight);
                propertyRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                if (!targetType.IsInstanceOfType(target.objectReferenceValue))
                {
                    target.objectReferenceValue = null;
                    target.serializedObject.ApplyModifiedProperties();
                    target.serializedObject.Update();
                }
                var obj = EditorGUI.ObjectField(targetFieldRect, target.objectReferenceValue, targetType, true);
                if (obj != target.objectReferenceValue)
                {
                    target.objectReferenceValue = obj;
                    target.serializedObject.ApplyModifiedProperties();
                    target.serializedObject.Update();
                }
                GUI.enabled = lastGUIState;
            }
            
            EndDrawBox(ref propertyRect);
        }

        public static void BeginDrawBox(ref Rect rect, ref Rect propertyRect)
        {
            EditorGUI.DrawRect(rect, new Color(0, 0, 0, 0.2f));
            rect.x += BOX_PADDING;
            rect.y += BOX_PADDING;
            rect.width -= BOX_PADDING * 2;
            rect.height -= BOX_PADDING * 2;
            propertyRect.y += BOX_PADDING;
        }

        public static void EndDrawBox(ref Rect propertyRect)
        {
            propertyRect.y += BOX_PADDING;
        }
        
        public static float GetHeaderHeight(SerializedProperty target)
        {
            var height = EditorGUIUtility.singleLineHeight + BOX_PADDING * 2;
            if (target != null)
                height += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

            return height;
        }
        
        private static ItemEntry[] GetItems(Type selectedType, out string selectedPath)
        {
            var types = TypeCache.GetTypesWithAttribute<ScriptableTweenPathAttribute>()
                .Where(type => typeof(IScriptableTweenItem).IsAssignableFrom(type))
                .Select(type => (type, type.GetCustomAttribute<ScriptableTweenPathAttribute>()))
                .OrderBy(args => args.Item2.Order)
                .Select(args => (args.type, args.Item2.Path))
                .ToArray();
            selectedPath = types.First(t => t.type == selectedType).Path;
            return types.Select(t => new ItemEntry(t.type, t.Path)).ToArray();
        }

        private static bool GetFoldoutState(SerializedProperty property)
        {
            if (_foldoutStates.ContainsKey(property.propertyPath))
                return _foldoutStates[property.propertyPath];
            return false;
        }

        private static void SetFoldoutState(SerializedProperty property, bool state)
        {
            var keys = _foldoutStates.Keys.ToArray();
            foreach (var key in keys)
                _foldoutStates[key] = false;
            _foldoutStates[property.propertyPath] = state;
        }
    }
}