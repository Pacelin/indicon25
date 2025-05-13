using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace TSS.Utils.Editor
{
    [CustomPropertyDrawer(typeof(SerializedBigInteger))]
    public class SerializedBigIntegerPropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var labelStyle = GUI.skin.label;
            labelStyle.alignment = TextAnchor.MiddleRight;
            var representation = property.FindPropertyRelative("_representation");
            var propertyRect = label == GUIContent.none ? position : EditorGUI.PrefixLabel(position, label);
            var dimensions = GetDimensionsCount(out int rows);
            var dimensionsPerRow = Mathf.CeilToInt(1f * dimensions / rows);
            var elementWidth = (propertyRect.width - EditorGUIUtility.standardVerticalSpacing * (dimensionsPerRow - 1)) 
                               / dimensionsPerRow;
            if (representation.arraySize != dimensions)
                representation.arraySize = dimensions;

            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < dimensionsPerRow; c++)
                {
                    var dimension = dimensions - 1 - r * dimensionsPerRow - c;
                    if (dimension < 0)
                        return;
                    var myProp = representation.GetArrayElementAtIndex(c + r * dimensionsPerRow);
                    var myRect = new Rect(
                        propertyRect.x + (elementWidth + EditorGUIUtility.standardVerticalSpacing) * c,
                        propertyRect.y + (EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing) * r,
                        elementWidth, EditorGUIUtility.singleLineHeight);
                    var newInt = Mathf.Clamp(EditorGUI.IntField(myRect, GUIContent.none, myProp.intValue), 0, 999);
                    if (newInt != myProp.intValue)
                        myProp.intValue = newInt;
                    EditorGUI.LabelField(
                        new Rect(myRect.xMax - 22, myRect.y, 20, EditorGUIUtility.singleLineHeight),
                        TSSUtils.GetBigIntLetter(dimension), labelStyle);
                }
            }
            property.serializedObject.ApplyModifiedProperties();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            GetDimensionsCount(out int rows);
            return EditorGUIUtility.singleLineHeight * rows + EditorGUIUtility.standardVerticalSpacing * (rows - 1);
        }

        private int GetDimensionsCount(out int rows)
        {
            var attr = fieldInfo.GetCustomAttribute<BigDimensionsAttribute>();
            if (attr != null)
            {
                rows = attr.Rows;
                return attr.Dimensions;
            }

            rows = 1;
            return 5;
        }
    }
    
}