using UnityEditor;
using UnityEngine;

namespace TSS.ContentManagement.Editor
{
    [InitializeOnLoad]
    internal class CMSAssetInspectorGUI
    {
        static CMSAssetInspectorGUI()
        {
            UnityEditor.Editor.finishedDefaultHeaderGUI += OnPostHeaderGUI;
        }

        private static void OnPostHeaderGUI(UnityEditor.Editor editor)
        {
            if (editor.targets.Length is <= 0 or > 1)
                return;
            if (editor.target is GameObject)
                return;
            
            var assetPath = AssetDatabase.GetAssetPath(editor.target);
            if (string.IsNullOrEmpty(assetPath))
                return;
            var asset = AssetDatabase.LoadAssetAtPath<Object>(assetPath);
            if (asset is not ScriptableObject &&
                asset is not SceneAsset &&
                asset is not GameObject)
                return;
            
            var assetGUID = AssetDatabase.AssetPathToGUID(assetPath);
            var assetIsAddressable = CMSAssetDatabase.AssetIsAddressable(assetGUID);

            if (!assetIsAddressable)
            {
                var oldGUIState = GUI.enabled;
                GUI.enabled = false;
                GUILayout.Toggle(false, "Include in CMS");
                GUI.enabled = oldGUIState;
            }
            else
            {
                var entry = CMSAssetDatabase.FindEntry(assetGUID);
                var hasEntry = entry.IsValid();
                if (!hasEntry)
                {
                    if (GUILayout.Toggle(false, "Include in CMS"))
                        CMSAssetDatabase.MarkAsset(asset, assetGUID, asset.name);
                }
                else
                {
                    GUILayout.BeginHorizontal();
                    if (!GUILayout.Toggle(true, "Include in CMS"))
                        CMSAssetDatabase.UnmarkAsset(assetGUID);

                    var newName = EditorGUILayout.DelayedTextField(entry.CMSName, GUILayout.ExpandWidth(true));
                    if (entry.CMSName != newName)
                        CMSAssetDatabase.RenameAsset(assetGUID, newName);
                    GUILayout.EndHorizontal();
                    
                    if (asset is GameObject assetGO)
                    {
                        if (EditorGUILayout.DropdownButton(new GUIContent(entry.ComponentName), FocusType.Passive,
                                GUILayout.ExpandWidth(true)))
                        {
                            var menu = new GenericMenu();
                            menu.AddItem(new GUIContent("GameObject"), entry.ComponentName == "GameObject", 
                                () => CMSAssetDatabase.ChangeComponentName(assetGUID, "GameObject"));
                            for (int i = 0; i < assetGO.GetComponentCount(); i++)
                            {
                                var component = assetGO.GetComponentAtIndex(i);
                                var typeName = component.GetType().Name;
                                menu.AddItem(new GUIContent(typeName), entry.ComponentName == typeName,
                                    () => CMSAssetDatabase.ChangeComponentName(assetGUID, typeName, component.GetType().Namespace));
                            }
                            menu.ShowAsContext();
                        }
                    }
                }
            }
        }
    }
}