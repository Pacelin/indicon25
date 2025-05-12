#if TSS_FMOD
using DG.Tweening;
using TSS.Audio;
using UnityEngine;

namespace TSS.Tweening
{
    [System.Serializable]
    [ScriptableTweenPath("FMOD/Sound", 121)]
    public class ScriptableTweenFMOD_DOSound : IScriptableTweenItemNoTarget
    {
        [SerializeField] private ETweenConnectBehaviour _connectBehaviour;
        [Box("FMOD")]
        [Order(10)] 
        [SerializeField]
        private SoundEvent _sound;

        public void AddTween(Sequence sequence)
        {
            if (_connectBehaviour == ETweenConnectBehaviour.Append)
                sequence.AppendCallback(_sound.PlayOneShot);
            else
                sequence.JoinCallback(_sound.PlayOneShot);
        }
    }
    
    [System.Serializable]
    [ScriptableTweenPath("FMOD/Sound Component", 122)]
    [NoFoldout]
    public class ScriptableTweenFMOD_DOSoundComponent : IScriptableTweenItem<SoundEventComponent>
    {
        [SerializeField] private ETweenConnectBehaviour _connectBehaviour;

        public void AddTween(Sequence sequence, SoundEventComponent obj)
        {
            if (!obj)
                return;
            if (_connectBehaviour == ETweenConnectBehaviour.Append)
                sequence.AppendCallback(obj.PlayOneShot);
            else
                sequence.JoinCallback(obj.PlayOneShot);
        }
    }
}
#endif