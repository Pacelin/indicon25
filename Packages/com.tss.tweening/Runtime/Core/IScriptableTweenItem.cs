using System;
using DG.Tweening;
using Object = UnityEngine.Object;

namespace TSS.Tweening
{
    internal interface IScriptableTweenItem
    {
        Type TargetType { get; }

        void AddTween(Sequence sequence, Object obj);
    }

    internal interface IScriptableTweenItem<in T> : IScriptableTweenItem where T : Object
    {
        Type IScriptableTweenItem.TargetType => typeof(T);

        void IScriptableTweenItem.AddTween(Sequence sequence, Object obj) => AddTween(sequence, (T) obj);
        void AddTween(Sequence sequence, T obj);
    }
}