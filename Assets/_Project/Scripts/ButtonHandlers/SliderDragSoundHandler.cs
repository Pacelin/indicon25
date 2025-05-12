using TSS.Tweening.UI;
using TSS.Audio;
using UnityEngine;

namespace Siberian25.UI.ButtonHandlers
{
    public class SliderDragSoundHandler : SliderHandlerBase
    {
        [SerializeField] private SoundEvent _dragSound;

        protected override void OnDrag() => _dragSound.PlayOneShot();
        protected override void OnHover() { }
    }
}