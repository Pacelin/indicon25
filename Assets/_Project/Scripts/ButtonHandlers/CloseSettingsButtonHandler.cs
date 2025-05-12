using TSS.Tweening.UI;

namespace Siberian25.UI.ButtonHandlers
{
    public class CloseSettingsButtonHandler : ButtonHandlerBase
    {
        protected override void OnClick() => SettingsMenu.Close();
        protected override void OnHover() { }
    }
}