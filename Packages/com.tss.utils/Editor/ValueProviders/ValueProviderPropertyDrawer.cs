using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace TSS.Utils.Editor.ValueProviders
{
    public abstract class ValueProviderPropertyDrawer : PropertyDrawer
    {
        private const int BUTTON_SPACE = 4;
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            InitProp(property);
            var buttonRect = new Rect(position.xMax - EditorGUIUtility.singleLineHeight, position.y, 
                EditorGUIUtility.singleLineHeight, EditorGUIUtility.singleLineHeight);
            var propRect = new Rect(position.x, position.y,
                position.width - EditorGUIUtility.singleLineHeight - BUTTON_SPACE, position.height);
            var rectForProp = EditorGUI.PrefixLabel(propRect,
                new GUIContent(label.text + LabelPostfix(property.managedReferenceValue)));
            if (EditorGUI.DropdownButton(buttonRect, GUIContent.none, FocusType.Passive))
                ShowMenu(property);
            DrawObject(rectForProp, property);

            property.serializedObject.ApplyModifiedProperties();
            property.serializedObject.Update();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            InitProp(property);
            var customHeight = GetInsideHeight(property);
            if (customHeight != 0)
                return customHeight;
            var rowsCount = GetRowsCount(property.managedReferenceValue);
            return rowsCount * EditorGUIUtility.singleLineHeight +
                   (rowsCount - 1) * EditorGUIUtility.standardVerticalSpacing;
        }

        protected abstract object GetDefaultObject();
        protected abstract void DrawObject(Rect rect, SerializedProperty property);
        protected virtual int GetRowsCount(object value) => 1;
        protected virtual float GetInsideHeight(SerializedProperty property) => 0;
        protected virtual string LabelPostfix(object value) => "";
        protected abstract IEnumerable<ValueProviderEntry> GetPossibleEntries();
        
        private void InitProp(SerializedProperty property)
        {
            if (property.managedReferenceValue != null)
                return;
            property.managedReferenceValue = GetDefaultObject();
            property.serializedObject.ApplyModifiedProperties();
            property.serializedObject.Update();
        }

        private void ShowMenu(SerializedProperty property)
        {
            var menu = new GenericMenu();
            foreach (var entry in GetPossibleEntries())
            {
                menu.AddItem(new GUIContent(entry.Name), false, () =>
                {
                    if (property.managedReferenceValue.GetType() != entry.Type)
                    {
                        property.managedReferenceValue = entry.ObjFactory();
                        property.serializedObject.ApplyModifiedProperties();
                        property.serializedObject.Update();
                    }
                });
            }
            menu.ShowAsContext();
        }

        protected Rect[] SplitColumns(Rect rect, int columnsCount)
        {
            var result = new Rect[columnsCount];
            var columnWidth = (rect.width - EditorGUIUtility.standardVerticalSpacing * (columnsCount - 1)) / columnsCount;
            for (int i = 0; i < columnsCount; i++)
            {
                result[i] = new Rect(rect.x + columnWidth * i + EditorGUIUtility.standardVerticalSpacing * i, rect.y,
                    columnWidth, rect.height);
            }
            return result;
        }
        
        protected Rect[] SplitRows(Rect rect, int rowsCount)
        {
            var result = new Rect[rowsCount];
            for (int i = 0; i < rowsCount; i++)
            {
                result[i] = new Rect(rect.x,
                    rect.y + i * EditorGUIUtility.singleLineHeight + i * EditorGUIUtility.standardVerticalSpacing,
                    rect.width, EditorGUIUtility.singleLineHeight);
            }

            return result;
        }
    }
}