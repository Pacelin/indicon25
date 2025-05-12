using TSS.Tweening.UI;

namespace Siberian25.UI.ButtonHandlers
{
    public class PauseButtonHandler : ButtonHandlerBase
    {
        protected override void OnClick() => PauseMenu.Open();
        protected override void OnHover() { }
    }
}