using TSS.Tweening.UI;
using TSS.Audio;
using UnityEngine;

namespace Siberian25.UI.ButtonHandlers
{
    public class ButtonSoundHandler : ButtonHandlerBase
    {
        [SerializeField] private SoundEvent _hoverSound;
        [SerializeField] private SoundEvent _clickSound;

        protected override void OnClick() => _clickSound.PlayOneShot();
        protected override void OnHover() => _hoverSound.PlayOneShot();
    }
}