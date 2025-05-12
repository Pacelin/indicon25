using TSS.Tweening.UI;
using UnityEngine;

namespace Siberian25.UI.ButtonHandlers.Social
{
    public class TelegramButtonHandler : ButtonHandlerBase
    {
        protected override void OnClick() => Application.OpenURL("https://t.me/thespinningsofa");
        protected override void OnHover() { }
    }
}