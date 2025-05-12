using System.Reflection;
using JetBrains.Annotations;
using UnityEditor;

namespace TSS.Utils.Editor
{
    public static class SerializedPropertyExtensions
    {
        [PublicAPI]
        public static (FieldInfo FieldInfo, System.Type Type) GetFieldInfoAndType(this SerializedProperty property)
        {
            return (ScriptAttributeUtility.GetFieldInfoFromProperty(property, out var type), type);
        }
    }
}