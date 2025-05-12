using TSS.Audio;

namespace Siberian25.UI
{
    public class SliderMasterVolumeHandler : SliderVolumeHandlerBase
    {
        protected override float GetVolume() => AudioSystem.Volumes.MasterVolume;
        protected override void SetVolume(float volume) => AudioSystem.Volumes.MasterVolume = volume;
    }
}