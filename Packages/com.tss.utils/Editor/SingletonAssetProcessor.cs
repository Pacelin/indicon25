using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace TSS.Utils.Editor
{
    [InitializeOnLoad]
    public static class SingletonAssetProcessor
    {
        static SingletonAssetProcessor()
        {
            var typesWithAttribute = 
                TypeCache.GetTypesWithAttribute<CreateSingletonAssetAttribute>();
            var scriptableObjectType = typeof(ScriptableObject);

            foreach (var type in typesWithAttribute)
            {
                if (!scriptableObjectType.IsAssignableFrom(type))
                    continue;
                var attribute = type.GetCustomAttribute<CreateSingletonAssetAttribute>();
                if (attribute.Address == null)
                    TSSEditorUtils.ValidateSO(type, attribute.Path);
                else
                    TSSEditorUtils.ValidateAddressableSO(type, attribute.Path, attribute.Address);
            }
        }
    }
}