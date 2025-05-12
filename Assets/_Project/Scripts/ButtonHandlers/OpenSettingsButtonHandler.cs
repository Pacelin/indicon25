using TSS.Tweening.UI;

namespace Siberian25.UI.ButtonHandlers
{
    public class OpenSettingsButtonHandler : ButtonHandlerBase
    {
        protected override void OnClick() => SettingsMenu.Open();

        protected override void OnHover() { }
    }
}