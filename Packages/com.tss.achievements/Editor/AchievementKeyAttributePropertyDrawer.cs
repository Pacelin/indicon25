using System.Linq;
using UnityEditor;
using UnityEngine;

namespace TSS.Achievements.Editor
{
    [CustomPropertyDrawer(typeof(AchievementKeyAttribute))]
    public class AchievementKeyAttributePropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var keys = AssetDatabase
                .LoadAssetAtPath<AchievementsConfig>("Assets/_Project/Configs/SO_Achievements.asset").Keys
                .OrderBy(k => k);
            if (!keys.Contains(property.stringValue))
            {
                property.stringValue = "";
                property.serializedObject.ApplyModifiedPropertiesWithoutUndo();
                property.serializedObject.Update();
            }
                
            var buttonRect = EditorGUI.PrefixLabel(position, label);
            var buttonText = string.IsNullOrEmpty(property.stringValue) ? "None" : property.stringValue;
            
            if (EditorGUI.DropdownButton(buttonRect, new GUIContent(buttonText), FocusType.Passive))
            {
                var menu = new GenericMenu();
                foreach (var key in keys)
                {
                    menu.AddItem(new GUIContent(key), false, () =>
                    {
                        property.stringValue = key;
                        property.serializedObject.ApplyModifiedProperties();
                        property.serializedObject.Update();
                    });
                }
                menu.ShowAsContext();
            }
        }
    }
}