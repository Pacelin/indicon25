using JetBrains.Annotations;
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
    public static class EditorUtils
    {
        private const BindingFlags Flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Default;
        
        /// <summary>
        /// Iterates through parent children, which are sorted using <see cref="OrderAttribute"/> attribute.
        /// </summary>
        /// <param name="parent">Parent to iterate.</param>
        /// <param name="excludeFields">Fields to exclude on iteration.</param>
        /// <returns></returns>
        [PublicAPI]
        public static IEnumerable<SerializedProperty> GetChildren(
            this SerializedProperty parent,
            params string[] excludeFields)
        {
            SerializedProperty it = parent.Copy();
            SerializedProperty itEnd = parent.Copy();
                
            if (!itEnd.NextVisible(false))
                itEnd = null;

            if (!it.NextVisible(true))
                it = null;
            
            // If has no children
            if (it == null || itEnd != null && it.propertyPath == itEnd.propertyPath)
                return Enumerable.Empty<SerializedProperty>();

            List<SerializedProperty> properties = new();
            
            do
            {
                if (SerializedProperty.EqualContents(it, itEnd))
                    break;
                if (excludeFields != null && excludeFields.Contains(it.name))
                    continue;

                properties.Add(it.Copy());
            } while (it.NextVisible(false));

            return properties.OrderBy(order => order.GetAttribute<OrderAttribute>()?.SortOrder ?? 0);
        }

        /// <summary>
        /// Returns properties which values are tracked using attributes
        /// like <see cref="ShowIfAttribute"/>, <see cref="HideIfAttribute"/>, etc.
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        public static IEnumerable<SerializedProperty> GetVisibilityTrackers(this SerializedProperty property)
        {
            return GetVisibilityTrackersWithDuplicates().Distinct();

            IEnumerable<SerializedProperty> GetVisibilityTrackersWithDuplicates()
            {
                IEnumerable<ShowIfAttribute> showIfAttributes = property.GetAttributes<ShowIfAttribute>();
                IEnumerable<HideIfAttribute> hideIfAttributes = property.GetAttributes<HideIfAttribute>();

                foreach (ShowIfAttribute showIfAttribute in showIfAttributes)
                    yield return property.GetSibling(showIfAttribute.FieldName);
                foreach (HideIfAttribute hideIfAttribute in hideIfAttributes)
                    yield return property.GetSibling(hideIfAttribute.FieldName);
            }
        }

        /// <summary>
        /// Checks whether or not the property is visible. It counts as visible if
        /// it does not have <see cref="HideInInspector"/>, and if attributes like
        /// <see cref="ShowIfAttribute"/>, <see cref="HideIfAttribute"/>, etc. are passed.
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        [PublicAPI]
        public static bool IsVisible(this SerializedProperty property)
        {
            var showIf = property.GetAttribute<ShowIfAttribute>();
            var hideIf = property.GetAttribute<HideIfAttribute>();
            var hideInInspector = property.GetAttribute<HideInInspector>();

            bool showIfPassed = true;
            bool hideIfPassed = true;
            bool hideInInspectorPassed = hideInInspector == null;
            
            if (showIf != null)
            {
                object showIfValue = property.GetSibling(showIf.FieldName).GetPropertyValue();
                showIfPassed = showIfValue.Equals(showIf.Value);
            }

            if (hideIf != null)
            {
                object hideIfValue = property.GetSibling(hideIf.FieldName).GetPropertyValue();
                hideIfPassed = !hideIfValue.Equals(hideIf.Value);
            }

            return showIfPassed && hideIfPassed && hideInInspectorPassed;
        }

        /// <summary>
        /// Finds a sibling of a property by fieldName, which is a property which has the same parent.
        /// </summary>
        /// <param name="property">Property to search from.</param>
        /// <param name="fieldName">Name of a sibling field.</param>
        /// <returns></returns>
        [PublicAPI]
        public static SerializedProperty GetSibling(this SerializedProperty property, string fieldName)
        {
            string[] pathSegments = property.propertyPath.Split('.')[..^1];

            string newPath = string.Join(".", pathSegments) + "." + fieldName;
            return property.serializedObject.FindProperty(newPath);
        }
        
        
        /// <summary>
        /// Iterates through parent children, which are sorted using <see cref="OrderAttribute"/> attribute.
        /// </summary>
        /// <param name="parent">Parent to iterate.</param>
        /// <param name="excludeFields">Fields to exclude on iteration.</param>
        /// <returns></returns>
        [PublicAPI]
        public static IEnumerable<SerializedProperty> GetChildren(
            this SerializedObject parent,
            params string[] excludeFields)
        {
            SerializedProperty it = parent.GetIterator();

            // No children
            if (!it.NextVisible(true))
                return Enumerable.Empty<SerializedProperty>();

            List<SerializedProperty> properties = new();
            
            do
            {
                if (excludeFields != null && excludeFields.Contains(it.name))
                    continue;

                properties.Add(it.Copy());
            } while (it.NextVisible(false));

            return properties.OrderBy(order => order.GetAttribute<OrderAttribute>()?.SortOrder ?? 0);
        }

        /// <summary>
        /// Creates default children property fields, subscribing to trackers value changes, and adds them to a container.
        /// </summary>
        /// <param name="container">Container to add property fields to.</param>
        /// <param name="parent">Parent to iterate.</param>
        /// <param name="excludeFields">Fields that need to be excluded.</param>
        [PublicAPI]
        public static void CreateChildrenPropertyFields(
            VisualElement container,
            SerializedObject parent,
            params string[] excludeFields)
        {
            CreatePropertyFields(container, GetChildren(parent, excludeFields));
        }
        
        
        /// <summary>
        /// Creates default children property fields, subscribing to trackers value changes, and adds them to a container.
        /// </summary>
        /// <param name="container">Container to add property fields to.</param>
        /// <param name="parent">Parent to iterate.</param>
        /// <param name="excludeFields">Fields that need to be excluded.</param>
        [PublicAPI]
        public static void CreateChildrenPropertyFields(
            VisualElement container,
            SerializedProperty parent,
            params string[] excludeFields)
        {
            CreatePropertyFields(container, GetChildren(parent, excludeFields));
        }
        
        private static void CreatePropertyFields(
            VisualElement container,
            IEnumerable<SerializedProperty> properties)
        {
            var propertyPathToFieldMap = new Dictionary<string, PropertyField>();
            
            foreach (SerializedProperty prop in properties)
            {
                var propertyField = new PropertyField(prop);
                propertyField.BindProperty(prop);
                container.Add(propertyField);
                propertyPathToFieldMap.Add(prop.propertyPath, propertyField);
            }

            foreach (SerializedProperty prop in properties)
            {
                IEnumerable<SerializedProperty> trackers = prop.GetVisibilityTrackers();
                
                SerializedProperty propCopy = prop.Copy();
                foreach (SerializedProperty tracker in trackers)
                {
                    PropertyField trackerField = propertyPathToFieldMap[tracker.propertyPath];
                    trackerField.RegisterValueChangeCallback(_ => UpdatePropertyVisibility(propCopy));
                }
            }

            return;

            void UpdatePropertyVisibility(SerializedProperty property)
            {
                PropertyField field = propertyPathToFieldMap[property.propertyPath];
                bool visible = property.IsVisible();
                field.style.display = new StyleEnum<DisplayStyle>(visible ? DisplayStyle.Flex : DisplayStyle.None);
            }
        }

        /// <summary>
        /// Checks if given property has attribute of type TAttribute.
        /// </summary>
        /// <param name="property">Property to check.</param>
        /// <typeparam name="TAttribute">Type of an attribute.</typeparam>
        /// <returns></returns>
        [PublicAPI]
        public static bool HasAttribute<TAttribute>(this SerializedProperty property) where TAttribute : Attribute
        {
            TAttribute attr = property.GetAttribute<TAttribute>();

            return attr != null;
        }
        
        /// <summary>
        /// Tries to find property with a specific attribute.
        /// </summary>
        /// <param name="obj">Parent to search from.</param>
        /// <param name="property">Returned property, if found.</param>
        /// <param name="attribute">Returned attribute, if found.</param>
        /// <param name="excludeFields">Fields to exclude search.</param>
        /// <typeparam name="TAttribute">Type of an attribute</typeparam>
        /// <returns></returns>
        [PublicAPI]
        public static bool TryGetPropertyWithAttribute<TAttribute>(
            this SerializedObject obj,
            out SerializedProperty property,
            out TAttribute attribute,
            params string[] excludeFields) where TAttribute : Attribute
        {
            foreach (SerializedProperty child in obj.GetChildren(excludeFields))
            {
                TAttribute attr = child.GetAttribute<TAttribute>();
                if (attr == null) continue;

                property = child;
                attribute = attr;
                return true;
            }

            property = null;
            attribute = null;
            return false;
        }

        [PublicAPI]
        [NotNull]
        public static IEnumerable<TAttribute> GetAttributes<TAttribute>(this SerializedProperty property) where TAttribute : Attribute
        {
            if (property == null) return Enumerable.Empty<TAttribute>();

            FieldInfo fi = property.GetPropertyFieldInfo();

            if (fi != null)
                return fi.GetCustomAttributes<TAttribute>();
            return Enumerable.Empty<TAttribute>();
        }

        [PublicAPI]
        public static TAttribute GetAttribute<TAttribute>(this SerializedProperty property) where TAttribute : Attribute
        {
            IEnumerable<TAttribute> attrs = GetAttributes<TAttribute>(property);
            return attrs.FirstOrDefault();
        }
        
        private static void GetFieldData(object target, string fieldName, out Type type, out object value, ref FieldInfo fi)
        {
            if (fieldName.StartsWith("data["))
            {
                int indexStart = fieldName.IndexOf('[');
                string indexString = fieldName.Substring(indexStart + 1, fieldName.IndexOf(']') - indexStart - 1);
                int arrayIndex = int.Parse(indexString);

                var array = (Array)target;
                
                value = array.GetValue(arrayIndex);
                type = value?.GetType() ?? array.GetType().GetElementType();
            }
            else
            {
                Type currentType = target.GetType();
                fi = currentType.GetFieldInSelfOrBaseType(fieldName, Flags);
                
                if (fi == null)
                    throw new Exception($"Field '{fieldName}' not found in type '{target.GetType()}'.");

                value = fi.GetValue(target);
                type = value?.GetType() ?? fi.FieldType;
            }
        }


        [CanBeNull]
        [PublicAPI]
        public static FieldInfo GetFieldInSelfOrBaseType(this Type type, string fieldName, BindingFlags flags)
        {
            FieldInfo fi;
            Type currentType = type;
            
            do
            {
                fi = currentType.GetField(fieldName, flags);
                currentType = currentType.BaseType;
            }
            while (fi == null && currentType != null);

            return fi;
        }
        
        [PublicAPI]
        public static Type GetPropertyType(this SerializedProperty property)
        {
            object currentObject = property.serializedObject.targetObject;
            Type currentType = currentObject.GetType();
            FieldInfo fi = null;
            
            string[] pathParts = property.propertyPath.Replace("Array.", string.Empty).Split(".");

            foreach (string part in pathParts)
                GetFieldData(currentObject, part, out currentType, out currentObject, ref fi);

            return currentType;
        }

        [PublicAPI]
        public static object GetPropertyValue(this SerializedProperty property)
        {
            object currentObject = property.serializedObject.targetObject;
            FieldInfo fi = null;
            
            string[] pathParts = property.propertyPath.Replace("Array.", string.Empty).Split(".");

            foreach (string part in pathParts)
                GetFieldData(currentObject, part, out _, out currentObject, ref fi);

            return currentObject;
        }

        [PublicAPI]
        public static void SetPropertyValue(this SerializedProperty property, object value)
        {
            object currentObject = property.serializedObject.targetObject;
            Type currentType = currentObject.GetType();
            FieldInfo currentField = null;
            
            string[] pathParts = property.propertyPath.Replace("Array.", string.Empty).Split(".");

            for (int i = 0; i < pathParts.Length - 1; i++)
                GetFieldData(currentObject, pathParts[i], out currentType, out currentObject, ref currentField);

            string lastPart = pathParts[^1]; 
            
            if (lastPart.StartsWith("data["))
            {
                int indexStart = lastPart.IndexOf('[');
                string indexString = lastPart.Substring(indexStart + 1, lastPart.IndexOf(']') - indexStart - 1);
                int arrayIndex = int.Parse(indexString);

                var array = (Array)currentObject;
                array.SetValue(value, arrayIndex);
            }
            else
            {
                currentField = currentType.GetFieldInSelfOrBaseType(lastPart, Flags);
                
                if (currentField == null)
                    throw new Exception($"Field '{lastPart}' not found in type '{currentObject.GetType()}'.");

                currentField.SetValue(currentObject, value);
            }
        }

        [PublicAPI]
        [CanBeNull]
        public static FieldInfo GetPropertyFieldInfo(this SerializedProperty property)
        {
            object currentObject = property.serializedObject.targetObject;
            FieldInfo fi = null;
            
            string[] pathParts = property.propertyPath.Replace("Array.", string.Empty).Split(".");
            
            foreach (string part in pathParts)
                GetFieldData(currentObject, part, out _, out currentObject, ref fi);

            return fi;
        }
    }
}