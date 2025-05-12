using Cysharp.Threading.Tasks;
using TSS.Tweening.UI;
using TSS.ContentManagement;
using TSS.Core;
using TSS.SceneManagement;

namespace Siberian25.UI.ButtonHandlers
{
    public class PlayButtonHandler : ButtonHandlerBase
    {
        protected override void OnClick() => SceneManager.Scene(CMS.Scenes.Game).Single().Load(Runtime.CancellationToken).Forget();
        protected override void OnHover() { }
    }
}