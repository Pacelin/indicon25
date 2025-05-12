using TSS.Tweening.UI;
using UnityEngine;

namespace Siberian25.UI.ButtonHandlers.Social
{
    public class BlueskyButtonHandler : ButtonHandlerBase
    {
        protected override void OnClick() => Application.OpenURL("https://bsky.app/profile/thespinningsofa.bsky.social");
        protected override void OnHover() { }
    }
}