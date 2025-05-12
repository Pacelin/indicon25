using DG.Tweening;
using UnityEngine;

namespace TSS.Tweening
{
    [System.Serializable]
    [ScriptableTweenPath("Play ParticleSystem", 101)]
    public class ScriptableTweenParticles_DOPlay : IScriptableTweenItem<ParticleSystem>
    {
        public void AddTween(Sequence sequence, ParticleSystem obj)
        {
            if (!obj)
                return;
            sequence
                .AppendCallback(obj.Play)
                .AppendInterval(obj.main.duration);
        }
    }
}
