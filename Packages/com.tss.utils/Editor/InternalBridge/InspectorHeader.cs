using UnityEditor;
using UnityEngine;

namespace TSS.Utils.Editor
{
    public static class InspectorHeader
    {
        public static Rect DrawCustomHeader(UnityEditor.Editor editor, 
          Texture2D texture, string header, float leftMargin = 0)
        { 
            GUILayout.BeginHorizontal(EditorStyles.inspectorBig);
            GUILayout.Space(38f);
            GUILayout.BeginVertical();
            GUILayout.Space(21f);
            GUILayout.BeginHorizontal();
            if (leftMargin > 0f)
                GUILayout.Space(leftMargin);
            if (editor)
                editor.OnHeaderControlsGUI();
            else
                EditorGUILayout.GetControlRect();
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
            Rect lastRect = GUILayoutUtility.GetLastRect();
            Rect r = new Rect(lastRect.x + leftMargin, lastRect.y, lastRect.width - leftMargin, lastRect.height);
            Rect rect1 = new Rect(r.x + 6f, r.y + 6f, 32f, 32f);
            if ((bool) (UnityEngine.Object) editor)
              OnHeaderIconGUI(rect1, texture);
            else
              GUI.Label(rect1, (Texture) AssetPreview.GetMiniTypeThumbnail(typeof (UnityEngine.Object)), EditorStyles.centeredGreyMiniLabel);
            if ((bool) (UnityEngine.Object) editor)
              editor.DrawPostIconContent(rect1);
            float lineHeight = EditorGUI.lineHeight;
            Rect rect2;
            if ((bool) (UnityEngine.Object) editor)
            {
              Rect rect3 = editor.DrawHeaderHelpAndSettingsGUI(r);
              float x = r.x + 44f;
              rect2 = new Rect(x, r.y + 6f, (float) ((double) rect3.x - (double) x - 4.0), lineHeight);
            }
            else
              rect2 = new Rect(r.x + 44f, r.y + 6f, r.width - 44f, lineHeight);
            if ((bool) (UnityEngine.Object) editor && editor.hasUnsavedChanges && !string.IsNullOrEmpty(header))
              header += " *";
            if ((bool) (UnityEngine.Object) editor)
              OnHeaderTitleGUI(rect2, header);
            else
              GUI.Label(rect2, header, EditorStyles.largeLabel);
            bool enabled = GUI.enabled;
            GUI.enabled = true;
            UnityEngine.Event current = UnityEngine.Event.current;
            bool flag = (UnityEngine.Object) editor != (UnityEngine.Object) null && current.type == UnityEngine.EventType.MouseDown && current.button == 1 && r.Contains(current.mousePosition);
            GUI.enabled = enabled;
            if (flag)
            {
              EditorUtility.DisplayObjectContextMenu(new Rect(current.mousePosition.x, current.mousePosition.y, 0.0f, 0.0f), editor.targets, 0);
              current.Use();
            }
            return lastRect;
        }
        
        private static void OnHeaderTitleGUI(Rect titleRect, string header)
        {
            titleRect.yMin -= 2f;
            titleRect.yMax += 2f;
            var style = EditorStyles.largeLabel;
            style.richText = true;
            GUI.Label(titleRect, header, style);
        }
        
        private static void OnHeaderIconGUI(Rect iconRect, Texture2D image)
        {
            GUI.Label(iconRect, image, EditorStyles.centeredGreyMiniLabel);
        }
    }
}