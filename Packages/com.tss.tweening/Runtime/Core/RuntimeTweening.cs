using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using JetBrains.Annotations;
#if TSS_CORE
using TSS.Core;
#endif

namespace TSS.Tweening
{
#if TSS_CORE
    [UsedImplicitly]
    [RuntimeOrder(ERuntimeOrder.SystemRegistration)]
    public class RuntimeTweening : IRuntimeLoader
    {
        public UniTask Initialize(CancellationToken cancellationToken)
        {
#if UNITY_EDITOR
            DOTween.Init(false, false, LogBehaviour.Verbose);
#else            
            DOTween.Init(false, false, LogBehaviour.ErrorsOnly);
#endif
            DOTween.defaultAutoPlay = AutoPlay.None;
            DOTween.defaultAutoKill = false;
            return UniTask.CompletedTask;
        }

        public void Dispose() { }
    } 
#else
    [PublicAPI]
    public static class RuntimeTweening
    {
        public static void Initialize()
        {
#if UNITY_EDITOR
            DOTween.Init(false, false, LogBehaviour.Verbose);
#else            
            DOTween.Init(false, false, LogBehaviour.ErrorsOnly);
#endif
            DOTween.defaultAutoPlay = AutoPlay.None;
            DOTween.defaultAutoKill = false;
        }
    }
#endif
}