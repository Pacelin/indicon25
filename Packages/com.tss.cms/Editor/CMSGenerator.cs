using System.Collections.Generic;
using System.Linq;
using Scriban;
using TSS.Utils.Editor;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEngine;

namespace TSS.ContentManagement.Editor
{
    [System.Serializable]
    internal class GenerationData
    {
        public List<FolderGenerationData> folders;
        public List<ItemGenerationData> items;
        public List<string> namespaces;
        public bool print_loader;
    }
    
    [System.Serializable]
    internal class FolderGenerationData
    {
        public string name;
        public List<FolderGenerationData> subfolders;
        public List<ItemGenerationData> items;
    }

    [System.Serializable] 
    internal class ItemGenerationData
    {
        public string type;
        public string name;
        public string address;
        public bool is_address;
        public bool get_component;
    }

    internal static class CMSGenerator
    {
        private const string TEMPLATE_PATH = "Packages/com.tss.cms/Editor/cms_template.txt";
        private const string GENERATION_PATH = "Assets/TSS/CMS.Generated.cs";
        
        public static void Generate()
        {
            AssetDatabase.StartAssetEditing();
            try
            {
                var database = AssetDatabase.LoadAssetAtPath<CMSAssetDatabase>(CMSAssetDatabase.ASSET_PATH);
                var templateAsset = AssetDatabase.LoadAssetAtPath<TextAsset>(TEMPLATE_PATH);
                var template = Template.Parse(templateAsset.text);
                var model = CollectData(database);
                var render = template.Render(model);
                TSSEditorUtils.WriteFile(GENERATION_PATH, render);
            }
            finally
            {
                AssetDatabase.StopAssetEditing();
                AssetDatabase.Refresh();
            }
        }

        private static GenerationData CollectData(CMSAssetDatabase database)
        {
            List<ItemGenerationData> items = new List<ItemGenerationData>();
            List<FolderGenerationData> folders = new List<FolderGenerationData>();
            List<string> namespaces = new List<string>()
            {
                "System.Threading",
                "Cysharp.Threading.Tasks",
                "UnityEngine.AddressableAssets",
                "JetBrains.Annotations",
                "UnityEngine"
            };

            FolderGenerationData GetFolder(string[] split)
            {
                FolderGenerationData currentFolder = folders.FirstOrDefault(f => f.name == split[0]);
                if (currentFolder == null)
                {
                    currentFolder = new FolderGenerationData()
                    {
                        name = split[0],
                        subfolders = new List<FolderGenerationData>(),
                        items = new List<ItemGenerationData>()
                    };
                    folders.Add(currentFolder);
                }
                
                for (int i = 1; i < split.Length - 1; i++)
                {
                    var folderName = split[i];
                    var nextFolder = currentFolder.subfolders.FirstOrDefault(f => f.name == folderName);
                    if (nextFolder == null)
                    {
                        nextFolder = new FolderGenerationData()
                        {
                            name = folderName,
                            subfolders = new List<FolderGenerationData>(),
                            items = new List<ItemGenerationData>()
                        };
                        currentFolder.subfolders.Add(nextFolder);
                    }
                    currentFolder = nextFolder;
                }

                return currentFolder;
            }

            var entries = database.Entries.ToArray();
            foreach (var entry in entries)
            {
                var assetPath = AssetDatabase.GUIDToAssetPath(entry.GUID);
                if (assetPath == null)
                {
                    CMSAssetDatabase.UnmarkAsset(entry.GUID);
                    continue;
                }
                var asset = AssetDatabase.LoadAssetAtPath<Object>(assetPath);
                var addressable = AddressableAssetSettingsDefaultObject.Settings.FindAssetEntry(entry.GUID);
                if (addressable == null)
                {
                    CMSAssetDatabase.UnmarkAsset(entry.GUID);
                    continue;
                }
                
                var item = new ItemGenerationData();
                item.name = entry.CMSName;
                item.address = addressable.address;
                if (asset is SceneAsset)
                {
                    item.type = "string";
                    item.is_address = true;
                    item.get_component = false;
                }
                else if (asset is GameObject)
                {
                    item.type = entry.ComponentName;
                    item.is_address = false;
                    item.get_component = entry.ComponentName != "GameObject";
                    if (item.get_component)
                        namespaces.Add(entry.Namespace);
                }
                else
                {
                    item.type = asset.GetType().Name;
                    item.is_address = false;
                    item.get_component = false;
                    namespaces.Add(asset.GetType().Namespace);
                }

                if (entry.CMSName.Contains("/"))
                {
                    var split = entry.CMSName.Split("/");
                    item.name = split[^1];
                    var folder = GetFolder(split);
                    folder.items.Add(item);
                }
                else
                {
                    items.Add(item);
                }
            }

            return new GenerationData()
            {
                folders = folders,
                namespaces = namespaces,
                items = items,
#if TSS_CORE
                print_loader = true
#else
                print_loader = false
#endif
            };
        }
    }
}