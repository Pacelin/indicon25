using mixpanel;
using TSS.Tweening.UI;
using UnityEngine;

namespace Siberian25.UI.ButtonHandlers
{
    public class MixpanelScriptableButtonHandler : ButtonHandlerBase
    {
        [SerializeField] private string _event;

        protected override void OnClick() => Mixpanel.Track(_event);
        protected override void OnHover() { }
    }
}