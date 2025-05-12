using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace TSS.Utils.Editor
{
    public static partial class TSSEditorUtils
    {
        public static T GetCustomAttribute<T>(this SerializedProperty property)
            where T : Attribute
        {
            var (fieldInfo, _) = property.GetFieldInfoAndType();
            return fieldInfo.GetCustomAttribute<T>();
        }

        public static object GetDeclaringObject(this SerializedProperty property, bool ignoreArrays = true)
        {
            return PropertyUtility.GetDeclaringObject(property, ignoreArrays);
        }

        public static SerializedProperty GetParentProperty(this SerializedProperty property)
        {
            return PropertyUtility.GetParentProperty(property);
        }

        public static object GetPropertyValue(this SerializedProperty property)
        {
            return property.GetPropertyValue(GetFieldInfo(property), GetDeclaringObject(property));
        }
        
        public static Type GetDeclaringType(this SerializedProperty property)
        {
            var fieldInfo = property.GetFieldInfo();
            return fieldInfo.DeclaringType;
        }
        
        public static FieldInfo GetFieldInfo(this SerializedProperty property)
        {
            var (fieldInfo, _) = property.GetFieldInfoAndType();
            return fieldInfo;
        }
        
        public static Type GetFieldType(this SerializedProperty property)
        {
            var (_, type) = property.GetFieldInfoAndType();
            return type;
        }
    }
}