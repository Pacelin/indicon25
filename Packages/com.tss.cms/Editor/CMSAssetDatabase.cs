using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEngine;

namespace TSS.ContentManagement.Editor
{
    internal class CMSAssetDatabase : ScriptableObject
    {
        public const string ASSET_PATH = "Assets/TSS/CMSDatabase.asset";
        private const string ASSET_DIR = "Assets/TSS";

        private static CMSAssetDatabase Instance
        {
            get
            {
                if (!_instance)
                {
                    if (!AssetDatabase.AssetPathExists(ASSET_PATH))
                    {
                        if (!Directory.Exists(ASSET_DIR))
                            Directory.CreateDirectory(ASSET_DIR);
                        _instance = CreateInstance<CMSAssetDatabase>();
                        AssetDatabase.CreateAsset(_instance, ASSET_PATH);
                        AssetDatabase.Refresh();
                    }
                    else
                    {
                        _instance = AssetDatabase.LoadAssetAtPath<CMSAssetDatabase>(ASSET_PATH);
                    }
                }
                return _instance;
            }
        }

        private static CMSAssetDatabase _instance;

        public CMSAssetEntry[] Entries => _entries;
        
        [SerializeField] private CMSAssetEntry[] _entries;

        public static bool AssetIsAddressable(string guid)
        { 
            var entry = AddressableAssetSettingsDefaultObject.Settings.FindAssetEntry(guid);
            if (entry == null)
                return false;
            return true;
        }

        public static void MarkAsset(Object asset, string assetGUID, string name)
        {
            var serialized = new SerializedObject(Instance);
            var entriesProp = serialized.FindProperty(nameof(_entries));
            entriesProp.InsertArrayElementAtIndex(entriesProp.arraySize);
            var element = entriesProp.GetArrayElementAtIndex(entriesProp.arraySize - 1);
            element.FindPropertyRelative("GUID").stringValue = assetGUID;
            element.FindPropertyRelative("CMSName").stringValue = name;
            if (asset is GameObject)
            {
                element.FindPropertyRelative("ComponentName").stringValue = "GameObject";
                element.FindPropertyRelative("Namespace").stringValue = null;
            }
            serialized.ApplyModifiedProperties();
        }
        
        public static void UnmarkAsset(string assetGUID)
        {
            var entry = FindEntry(assetGUID);
            var index = System.Array.IndexOf(Instance._entries, entry);
            var serialized = new SerializedObject(Instance);
            var entriesProp = serialized.FindProperty(nameof(_entries));
            
            entriesProp.DeleteArrayElementAtIndex(index);
            serialized.ApplyModifiedProperties();
        }

        public static void ChangeComponentName(string assetGUID, string componentName)
        {
            var entry = FindEntry(assetGUID);
            var index = System.Array.IndexOf(Instance._entries, entry);
            var serialized = new SerializedObject(Instance);
            var entriesProp = serialized.FindProperty(nameof(_entries));
            
            entriesProp.GetArrayElementAtIndex(index).FindPropertyRelative("ComponentName").stringValue = componentName;
            entriesProp.GetArrayElementAtIndex(index).FindPropertyRelative("Namespace").stringValue = null;
            serialized.ApplyModifiedProperties();            
        }
        
        public static void ChangeComponentName(string assetGUID, string componentName, string @namespace)
        {
            var entry = FindEntry(assetGUID);
            var index = System.Array.IndexOf(Instance._entries, entry);
            var serialized = new SerializedObject(Instance);
            var entriesProp = serialized.FindProperty(nameof(_entries));
            
            entriesProp.GetArrayElementAtIndex(index).FindPropertyRelative("ComponentName").stringValue = componentName;
            entriesProp.GetArrayElementAtIndex(index).FindPropertyRelative("Namespace").stringValue = @namespace;
            serialized.ApplyModifiedProperties();            
        }
        
        public static void RenameAsset(string assetGUID, string newName)
        {
            if (string.IsNullOrWhiteSpace(newName))
                return;
            const string VALID_CHARS = "qwertyuiopasdfghjklzxcvbnmQWERTYUIOPASDFGHJKLZXCVBNM/";
            if (newName.Any(ch => !VALID_CHARS.Contains(ch)))
                return;
            
            var entry = FindEntry(assetGUID);
            var index = System.Array.IndexOf(Instance._entries, entry);
            var serialized = new SerializedObject(Instance);
            var entriesProp = serialized.FindProperty(nameof(_entries));
            
            entriesProp.GetArrayElementAtIndex(index).FindPropertyRelative("CMSName").stringValue = newName;
            serialized.ApplyModifiedProperties();
        }
        
        public static CMSAssetEntry FindEntry(string assetGUID)
        {
            var entries = Instance._entries;
            if (entries == null)
                return default;
            
            foreach (var entry in entries)
                if (entry.GUID == assetGUID)
                    return entry;
            return default;
        }
    }
}