using System.Linq;
using TSS.Achievements.View;
using UnityEditor;
using UnityEditor.Search;
using UnityEngine;

namespace TSS.Achievements.Editor
{
    [CustomPropertyDrawer(typeof(AchievementContainerAttribute))]
    public class AchievementContainerAttributePropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var searchList = SearchService.Request($"p: t:prefab t:{nameof(AchievementNotificationCollectionView)}",
                SearchFlags.Synchronous);
            var assets = searchList.Select(item => item.ToObject<AchievementNotificationCollectionView>());
            var keys = assets.SelectMany(asset => asset.GetContainerKeys())
                .Distinct().ToArray();
            if (!keys.Contains(property.stringValue))
            {
                property.stringValue = keys.Length > 0 ? keys[0] : "";
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