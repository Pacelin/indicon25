using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using UnityEditor;

namespace TSS.Utils.Editor
{
    public static class PropertyUtility
    {
        public static object GetDeclaringObject(this SerializedProperty property, bool ignoreArrays = true)
        {
            return GetDeclaringObject(property, property.serializedObject.targetObject, ignoreArrays);
        }

        private static object GetDeclaringObject(this SerializedProperty property, UnityEngine.Object target, bool ignoreArrays)
        {
            EnsureReflectionSafeness(property);

            var reference = target as object;
            var validReference = reference;
            var members = GetPropertyFieldTree(property);
            if (members.Length > 1)
            {
                for (var i = 0; i < members.Length - 1; i++)
                {
                    var treeField = members[i];
                    reference = GetTreePathReference(treeField, reference);
                    if (reference == null)
                    {
                        continue;
                    }

                    if (ignoreArrays && IsSerializableArrayType(reference))
                    {
                        continue;
                    }
                    validReference = reference;
                }
            }

            return validReference;
        }

        private static object GetTreePathReference(string treeField, object treeParent)
        {
            if (IsSerializableArrayElement(treeField, out var index))
            {
                if (treeParent is IList list)
                    return list[index];
            }

            const BindingFlags flags = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public;

            var fieldType = treeParent.GetType();
            FieldInfo fieldInfo = null;
            //NOTE: make sure to check in the base classes since there can be a private field/property
            while (fieldType != null)
            {
                fieldInfo = fieldType.GetField(treeField, flags);
                if (fieldInfo != null)
                {
                    break;
                }

                fieldType = fieldType.BaseType;
            }

            if (fieldInfo == null)
            {
                return null;
            }

            return fieldInfo.GetValue(treeParent);
        }
        
        public static object GetPropertyValue(this SerializedProperty property, FieldInfo fieldInfo, object declaringObject)
        {
            if (fieldInfo == null)
            {
                throw new ArgumentNullException(nameof(fieldInfo));
            }

            if (IsSerializableArrayElement(property, fieldInfo))
            {
                var index = GetPropertyElementIndex(property);
                var list = fieldInfo.GetValue(declaringObject) as IList;
                return list?[index];
            }
            
            return fieldInfo.GetValue(declaringObject);
        }

        public static SerializedProperty GetParentProperty(this SerializedProperty property)
        {
            if (property.depth == 0)
            {
                return null;
            }

            SerializedProperty parent = null;
            var members = GetPropertyFieldTree(property);
            if (members.Length > 1)
            {
                parent = property.serializedObject.FindProperty(members[0]);
                for (var i = 1; i < members.Length - 1; i++)
                {
                    var fieldName = members[i];
                    parent = IsSerializableArrayElement(fieldName, out var index)
                        ? parent.GetArrayElementAtIndex(index)
                        : parent.FindPropertyRelative(fieldName);
                }
            }

            return parent;
        }

        private static void EnsureReflectionSafeness(SerializedProperty property)
        {
            if (property.serializedObject.hasModifiedProperties)
                property.serializedObject.ApplyModifiedProperties();
        }

        private static bool IsSerializableArrayType(object target)
        {
            return IsSerializableArrayType(target.GetType());
        }
        
        private static bool IsSerializableArrayType(Type type)
        {
            return typeof(IList).IsAssignableFrom(type);
        }

        private static bool IsSerializableArrayElement(SerializedProperty property)
        {
            return property.propertyPath.EndsWith("]");
        }

        private static bool IsSerializableArrayElement(SerializedProperty property, FieldInfo fieldInfo)
        {
            return !property.isArray && IsSerializableArrayType(fieldInfo.FieldType);
        }

        private static bool IsSerializableArrayElement(string indexField, out int index)
        {
            if (indexField.StartsWith("["))
            {
                index = int.Parse(indexField.Substring(1, indexField.Length - 2));
                return true;
            }

            index = -1;
            return false;
        }
        
        private static int GetPropertyElementIndex(SerializedProperty element)
        {
            if (!IsSerializableArrayElement(element))
            {
                return -1;
            }

            var indexString = string.Empty;
            var propertyPath = element.propertyPath;
            for (var i = propertyPath.Length - 2; i >= 0; i--)
            {
                var character = propertyPath[i];
                if (character.Equals('['))
                {
                    break;
                }

                indexString = character + indexString;
            }
            return int.Parse(indexString);
        }

        private static string[] GetPropertyFieldTree(SerializedProperty property, bool ignoreArrayElements = false)
        {
            return GetPropertyFieldTree(property.propertyPath, ignoreArrayElements);
        }

        private static string[] GetPropertyFieldTree(string propertyPath, bool ignoreArrayElements)
        {
            return ignoreArrayElements
                ? propertyPath.Replace("Array.data[", "[").Split('.').Where(field => field[0] != '[').ToArray()
                : propertyPath.Replace("Array.data[", "[").Split('.');
        }
    }
}