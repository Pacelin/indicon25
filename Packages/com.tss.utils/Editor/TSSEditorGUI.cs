using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;

namespace TSS.Utils.Editor
{
    [PublicAPI]
    public static class TSSEditorGUI
    {
        private static double _doubleClickExpiration;
        private const double _doubleClickThreshold = 0.3;
        private static float _lastLabelGUI;
        
        public static void BeginSmallLabel(float width = 51)
        {
            _lastLabelGUI = EditorGUIUtility.labelWidth;
            EditorGUIUtility.labelWidth = width;
        }

        public static void EndSmallLabel()
        {
            EditorGUIUtility.labelWidth = _lastLabelGUI;
        }
        
        public static bool DrawInlineReadonly(this SerializedProperty property)
        {
            property = property.Copy();
            if (!property.hasChildren) return false;
            if (!property.NextVisible(true)) return false;
            var sr = property.serializedObject;
            
            EditorGUI.BeginChangeCheck();
            var previousGUIState = GUI.enabled;
            GUI.enabled = false;
            sr.UpdateIfRequiredOrScript();

            var depth = property.depth;
            do
            {
                if (property.depth < depth)
                    break;
                EditorGUILayout.PropertyField(property, true);
            } while (property.NextVisible(false));
            
            sr.ApplyModifiedProperties();
            GUI.enabled = previousGUIState;
            return EditorGUI.EndChangeCheck();
        }
        
        public static bool DrawInline(this SerializedProperty property)
        {
            property = property.Copy();
            if (!property.hasChildren) return false;
            if (!property.NextVisible(true)) return false;
            var sr = property.serializedObject;
            
            EditorGUI.BeginChangeCheck();
            sr.UpdateIfRequiredOrScript();

            var depth = property.depth;
            do
            {
                if (property.depth < depth)
                    break;
                EditorGUILayout.PropertyField(property, true);
            } while (property.NextVisible(false));
            
            sr.ApplyModifiedProperties();
            return EditorGUI.EndChangeCheck();
        }
        
        public static bool ContextualClick(Rect rect)
        {
            var evt = Event.current;
            return evt.type == EventType.ContextClick && rect.Contains(evt.mousePosition);
        }
        
        public static bool DoubleClickButton(Rect position, string content)
        {
            if (GUI.Button(position, content))
            {
                var temp = _doubleClickExpiration;
                _doubleClickExpiration = EditorApplication.timeSinceStartup + _doubleClickThreshold;
                return temp > EditorApplication.timeSinceStartup;
            }
            return false;
        }
        
        public static bool DoubleClickButton(Rect position, string content, GUIStyle style)
        {
            if (GUI.Button(position, content, style))
            {
                var temp = _doubleClickExpiration;
                _doubleClickExpiration = EditorApplication.timeSinceStartup + _doubleClickThreshold;
                return temp > EditorApplication.timeSinceStartup;
            }
            return false;
        }
        
        public static bool DoubleClickButton(Rect position, GUIContent content)
        {
            if (GUI.Button(position, content))
            {
                var temp = _doubleClickExpiration;
                _doubleClickExpiration = EditorApplication.timeSinceStartup + _doubleClickThreshold;
                return temp > EditorApplication.timeSinceStartup;
            }
            return false;
        }
        
        public static bool DoubleClickButton(Rect position, GUIContent content, GUIStyle style)
        {
            if (GUI.Button(position, content, style))
            {
                var temp = _doubleClickExpiration;
                _doubleClickExpiration = EditorApplication.timeSinceStartup + _doubleClickThreshold;
                return temp > EditorApplication.timeSinceStartup;
            }
            return false;
        }

        public static void DrawCustomHeader(UnityEditor.Editor editor, Texture2D icon, string header) =>
            InspectorHeader.DrawCustomHeader(editor, icon, header);
    }
}