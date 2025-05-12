using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace TSS.Core.Editor
{
    [CustomPropertyDrawer(typeof(DropdownAttribute))]
    public class DropdownPropertyDrawer : PropertyDrawer
    {
        [SerializeField]
        private StyleSheet _dropdownStyleSheet;
        
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            AssetDatabase.TryGetGUIDAndLocalFileIdentifier(property.serializedObject.targetObject, out string guid, out long localId);
            EditorPrefsBool expanded = new($"{nameof(DropdownPropertyDrawer)}/{property.propertyPath}/{guid}/{localId}");
            
            List<(string name, Type type)> choices = GetDropdownChoices(property);

            Foldout foldout = CreateFoldoutHeader(property, expanded, choices);
            foldout.styleSheets.Add(_dropdownStyleSheet);
            AddChildrenPropertyFields(property, foldout.contentContainer);
            
            return foldout;
        }

        private static List<(string name, Type type)> GetDropdownChoices(SerializedProperty property)
        {
            FieldInfo fi = property.GetPropertyFieldInfo();
            Type type = fi.FieldType.IsArray ? fi.FieldType.GetElementType() : fi.FieldType;
            var dropdownAttribute = property.GetAttribute<DropdownAttribute>();

            return GetDropdownChoices(type, dropdownAttribute.IncludeSelf, dropdownAttribute.IncludeNone).ToList();
        }
        
        private Foldout CreateFoldoutHeader(
            SerializedProperty property,
            EditorPrefsBool expanded,
            List<(string name, Type type)> choices)
        {
            var foldout = new Foldout
            {
                text = property.displayName,
                value = expanded.Value,
            };
            foldout.BindProperty(property);
            foldout.RegisterValueChangedCallback(evt => expanded.Value = !evt.newValue);

            DropdownField dropdown = CreateDropdown(property, choices, foldout);
            
            foldout.Q<Toggle>().contentContainer.Add(dropdown);

            return foldout;
        }

        private DropdownField CreateDropdown(
            SerializedProperty property,
            List<(string name, Type type)> choices, 
            Foldout foldout)
        {
            List<string> choiceNames = choices.Select(pair => pair.name).ToList();
            Type propType = property.GetPropertyValue()?.GetType();
            
            var dropdown = new DropdownField
            {
                label = string.Empty,
                choices = choiceNames,
            };
            dropdown.AddToClassList("dropdown");
            dropdown.RegisterValueChangedCallback(evt => ChoiceChanged(evt, property, choices, foldout));
            dropdown.SetValueWithoutNotify(choices.Where(pair => pair.type == propType).Select(pair => pair.name).FirstOrDefault() ?? "Name not found");
            
            return dropdown;
        }
        
        private void ChoiceChanged(
            ChangeEvent<string> evt,
            SerializedProperty property,
            List<(string name, Type type)> choices,
            Foldout foldout)
        {
            Type oldType = property.GetPropertyType();
            Type newType = choices.FirstOrDefault(pair => string.Equals(pair.name, evt.newValue)).type;

            if (oldType == newType) return;

            if (newType == null)
            {
                property.SetPropertyValue(default);
                property.serializedObject.ApplyModifiedProperties();
                property.serializedObject.Update();
                ClearChildrenPropertyFields(foldout.contentContainer);
                EditorUtility.SetDirty(property.serializedObject.targetObject);
                return;
            }

            object oldValue = property.GetPropertyValue();
            object newValue;

            if (typeof(ScriptableObject).IsAssignableFrom(newType))
                newValue = ScriptableObject.CreateInstance(newType);
            else
                newValue = Activator.CreateInstance(newType);

            property.SetPropertyValue(newValue);
            if (oldValue != null && newValue != null)
                EditorUtility.CopySerializedManagedFieldsOnly(oldValue, newValue);
            property.serializedObject.ApplyModifiedProperties();
            property.serializedObject.Update();
            EditorUtility.SetDirty(property.serializedObject.targetObject);
            
            ClearChildrenPropertyFields(foldout.contentContainer);
            AddChildrenPropertyFields(property, foldout.contentContainer);
        }
        
        private void ClearChildrenPropertyFields(VisualElement container)
        {
            container.Clear();
        }
        
        private void AddChildrenPropertyFields(SerializedProperty parent, VisualElement container)
        {
            Dictionary<string, PropertyField> propertyPathToFieldMap = new();
            
            foreach (SerializedProperty prop in parent.GetChildren())
            {
                var propertyField = new PropertyField(prop);
                propertyField.BindProperty(prop);
                container.Add(propertyField);
                propertyPathToFieldMap.Add(prop.propertyPath, propertyField);
            }

            foreach (SerializedProperty prop in parent.GetChildren())
            {
                IEnumerable<SerializedProperty> trackers = prop.GetVisibilityTrackers();
                
                SerializedProperty propCopy = prop.Copy();
                foreach (SerializedProperty tracker in trackers)
                {
                    PropertyField trackerField = propertyPathToFieldMap[tracker.propertyPath];
                    trackerField.RegisterValueChangeCallback(_ => UpdatePropertyVisibility(propCopy, propertyPathToFieldMap));
                }
            }
        }

        private void UpdatePropertyVisibility(SerializedProperty property, Dictionary<string, PropertyField> propertyPathToFieldMap)
        {
            PropertyField field = propertyPathToFieldMap[property.propertyPath];
            bool visible = property.IsVisible();
            field.style.display = new StyleEnum<DisplayStyle>(visible ? DisplayStyle.Flex : DisplayStyle.None);
        }
        
        private const string NoneChoiceName = "None";
        
        public static IEnumerable<(string name, Type type)> GetDropdownChoices(Type baseType, bool includeSelf, bool includeNone)
        {
            // Include None first
            if (includeNone)
                yield return (NoneChoiceName, null);

            TypeCache.TypeCollection derivedTypes = TypeCache.GetTypesDerivedFrom(baseType);

            IEnumerable<(string, Type type)> nonOrderedTypes =
                derivedTypes.
                    Where(type =>
                    {
                        // Disallow abstracts
                        if (type.IsAbstract)
                            return false;

                        return true;
                    }).
                    Select(type => (GetDisplayName(type), type));

            // Add Self
            if (includeSelf)
                nonOrderedTypes = nonOrderedTypes.Append((GetDisplayName(baseType), baseType));

            IOrderedEnumerable<(string, Type type)> orderedTypes = nonOrderedTypes.OrderBy(tuple => tuple.Item1);

            foreach ((string name, Type type) pair in orderedTypes)
            {
                yield return pair;
            }
        }

        private static string GetDisplayName(Type type)
        {
            return type.GetCustomAttribute<DropdownDisplayAttribute>()?.DisplayName ?? type.FullName;
        }
    }
}