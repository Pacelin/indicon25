using JetBrains.Annotations;
using TSS.Utils;
using UnityEditor.UIElements;

namespace TSS.Audio.Editor
{
    [UsedImplicitly]
    [EditorToolbarElement(EToolbarPanel.RightPanel, EToolbarAlign.Right)]
    internal class FMODAudioGenerateButton : ToolbarButton
    {
        public FMODAudioGenerateButton()
        {
            text = "Refresh FMOD";
            iconImage = FMODUtilsInternal.GetFMODStudioIcon();
            style.paddingLeft = 4;
            style.paddingRight = 4;
            clicked += OnClick;
        }
        ~FMODAudioGenerateButton()
        {
            clicked -= OnClick;
        }

        private void OnClick() =>
            FMODAudioGenerator.Generate();
    }
}