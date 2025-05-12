using System.Linq;
using UnityEditor;
using UnityEngine;

namespace TSS.Tweening.Editor
{
    public static partial class ScriptableTweenItemGUI
    {
        public static float GetContentHeight(SerializedProperty property)
        {
            if (!GetFoldoutState(property))
                return 0;
            if (property.managedReferenceValue is EmptyScriptableTweenItem)
                return 0;
            
            var boxes = CollectBoxes(property, p => !p.propertyPath.Contains("_connectBehaviour"));
            return boxes.Sum(b => b.GetHeight()) + (boxes.Count - 1) * EditorGUIUtility.standardVerticalSpacing + EditorGUIUtility.standardVerticalSpacing;
        }

        public static void DrawContent(ref Rect propertyRect, SerializedProperty property)
        {
            if (!GetFoldoutState(property))
                return;
            if (property.managedReferenceValue is EmptyScriptableTweenItem)
                return;

            var boxes = CollectBoxes(property, p => !p.propertyPath.Contains("_connectBehaviour"));
            var height = boxes.Sum(b => b.GetHeight()) + (boxes.Count - 1) * EditorGUIUtility.standardVerticalSpacing;
            var boxesRect = new Rect(propertyRect.x - 24, propertyRect.y + EditorGUIUtility.standardVerticalSpacing,
                propertyRect.width + 24, height);
            var boxesRectCopy = boxesRect;
            for (int i = 0; i < boxes.Count - 1; i++)
            {
                boxes[i].DrawGUI(ref boxesRect);
                boxesRect.y += EditorGUIUtility.standardVerticalSpacing;
            }
            boxes[^1].DrawGUI(ref boxesRect);

            propertyRect.y += boxesRect.y - boxesRectCopy.y;
        }
    }
}