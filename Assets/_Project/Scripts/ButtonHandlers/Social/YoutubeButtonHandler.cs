using TSS.Tweening.UI;
using UnityEngine;

namespace Siberian25.UI.ButtonHandlers.Social
{
    public class YoutubeButtonHandler : ButtonHandlerBase
    {
        protected override void OnClick() => Application.OpenURL("https://www.youtube.com/@thespinningsofa");
        protected override void OnHover() { }
    }
}