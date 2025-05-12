using TSS.Audio;
using UnityEngine;

namespace Siberian25.UI
{
    public class SliderVolumeHandler : SliderVolumeHandlerBase
    {
        [SerializeField] private int _configIndex;
        
        protected override float GetVolume() => AudioSystem.Volumes.GetVolume(_configIndex);
        protected override void SetVolume(float volume) => AudioSystem.Volumes.SetVolume(_configIndex, volume);
    }
}