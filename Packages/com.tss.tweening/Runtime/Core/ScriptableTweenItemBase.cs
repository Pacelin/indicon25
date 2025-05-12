using System;
using DG.Tweening;
using TSS.Utils;
using UnityEngine;
using Object = UnityEngine.Object;

namespace TSS.Tweening
{
    [Serializable]
    public abstract class ScriptableTweenItemBase<T> : IScriptableTweenItem<T> where T : Object
    {
        private bool ShowLoopType => _loopsCount is not IntValueProvider just || just.Get() != 1;
        [SerializeField] private ETweenConnectBehaviour _connectBehaviour = ETweenConnectBehaviour.Append;
        
        [Box("General")]
        [Order(2)]
        [SerializeField] private Ease _easing = Ease.OutQuad;
        
        [Box("General")]
        [Order(0)]
        [SerializeReference] private IFloatValueProvider _delay = new FloatValueProvider(0);
        [Box("General")]
        [Order(3)]
        [SerializeReference] private IIntValueProvider _loopsCount = new IntValueProvider(1);
        
        [Box("General")]
        [ShowIf(nameof(ShowLoopType))]
        [Order(4)]
        [SerializeField] private LoopType _loopType = LoopType.Restart;
        [Box("General")]
        [Order(5)]
        [SerializeField] private bool _ignoreTimescale;
        
        public void AddTween(Sequence sequence, T obj)
        {
            Tween tween = CreateTween(obj)
                .SetEase(_easing)
                .SetDelay(_delay.Get())
                .SetLoops(_loopsCount.Get(), _loopType)
                .SetUpdate(_ignoreTimescale);
            
            switch (_connectBehaviour)
            {
                case ETweenConnectBehaviour.Join:
                    sequence.Join(tween);
                    break;
                case ETweenConnectBehaviour.Append:
                default:
                    sequence.Append(tween);
                    break;
            }
        }

        protected abstract Tween CreateTween(T obj);
    }
}