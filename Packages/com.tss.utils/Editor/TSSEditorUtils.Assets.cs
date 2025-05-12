using System;
using System.IO;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEngine;

namespace TSS.Utils.Editor
{
    public static partial class TSSEditorUtils
    {
        public static void ValidateAssetPath(string path)
        {
            var directoryName = Path.GetDirectoryName(path);
            if (!Directory.Exists(directoryName))
                Directory.CreateDirectory(directoryName!);
        }

        public static void WriteFile(string filePath, string content)
        {
            ValidateAssetPath(filePath);
            File.WriteAllText(filePath, content);
        }
        
        public static T ValidateAddressableSO<T>(string path, string address, out bool created) where T : ScriptableObject
        {
            if (AssetDatabase.AssetPathExists(path))
            {
                created = false;
                return AssetDatabase.LoadAssetAtPath<T>(path);
            }

            ValidateAssetPath(path);
            var asset = ScriptableObject.CreateInstance<T>();
            AssetDatabase.CreateAsset(asset, path);
            AssetDatabase.SaveAssets();

            var settings = AddressableAssetSettingsDefaultObject.GetSettings(true);
            var entry = settings.CreateOrMoveEntry(AssetDatabase.AssetPathToGUID(path),
                settings.DefaultGroup, false, false);
            entry.SetAddress(address);
            
            created = true;
            return asset;
        }
        
        public static ScriptableObject ValidateSO(Type soType, string path)
        {
            if (AssetDatabase.AssetPathExists(path))
                return AssetDatabase.LoadAssetAtPath<ScriptableObject>(path);

            ValidateAssetPath(path);
            var asset = ScriptableObject.CreateInstance(soType);
            AssetDatabase.CreateAsset(asset, path);
            AssetDatabase.SaveAssets();

            return asset;
        }
        
        public static ScriptableObject ValidateAddressableSO(Type soType, string path, string address)
        {
            if (AssetDatabase.AssetPathExists(path))
                return AssetDatabase.LoadAssetAtPath<ScriptableObject>(path);

            ValidateAssetPath(path);
            var asset = ScriptableObject.CreateInstance(soType);
            AssetDatabase.CreateAsset(asset, path);
            AssetDatabase.SaveAssets();

            var settings = AddressableAssetSettingsDefaultObject.GetSettings(true);
            var entry = settings.CreateOrMoveEntry(AssetDatabase.AssetPathToGUID(path),
                settings.DefaultGroup, false, false);
            entry.SetAddress(address);
            
            return asset;
        }
    }
}