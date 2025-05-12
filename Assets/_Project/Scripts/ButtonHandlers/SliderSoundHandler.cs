using TSS.Tweening.UI;
using TSS.Audio;
using UnityEngine;

namespace Siberian25.UI.ButtonHandlers
{
    public class SliderSoundHandler : SliderHandlerBase
    {
        [SerializeField] private SoundEvent _hoverSound;

        protected override void OnDrag() { }
        protected override void OnHover() => _hoverSound.PlayOneShot();
    }
}