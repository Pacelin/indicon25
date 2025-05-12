using System;
using DG.Tweening;
using TSS.Utils;
using UnityEngine;
using Object = UnityEngine.Object;

namespace TSS.Tweening
{
    [Serializable] 
    public abstract class ScriptableTweenDurableItemBase<T> : ScriptableTweenItemBase<T> where T : Object
    {
        [Box("General")]
        [Order(1)]
        [SerializeReference] private IFloatValueProvider _duration = new FloatValueProvider(0.5f);
        
        protected sealed override Tween CreateTween(T obj) => CreateTween(obj, _duration.Get());
        protected abstract Tween CreateTween(T obj, float duration);
    }
}