using JetBrains.Annotations;
using TSS.Utils;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace TSS.ContentManagement.Editor
{
    [UsedImplicitly]
    [EditorToolbarElement(EToolbarPanel.RightPanel, EToolbarAlign.Right)]
    internal class CMSGenerateButton : ToolbarButton
    {
        public CMSGenerateButton()
        {
            text = "Refresh CMS";
            var icon = EditorGUIUtility.IconContent("icons/d_BuildSettings.Standalone.Small.png");
            iconImage = Background.FromTexture2D(icon.image as Texture2D);
            style.paddingLeft = 4;
            style.paddingRight = 4;
            clicked += OnClick;
        }
        ~CMSGenerateButton()
        {
            clicked -= OnClick;
        }

        private void OnClick() =>
            CMSGenerator.Generate();
    }
}