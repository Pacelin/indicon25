using System;
using DG.Tweening;
using Object = UnityEngine.Object;

namespace TSS.Tweening
{
    internal interface IScriptableTweenItemNoTarget : IScriptableTweenItem
    {
        Type IScriptableTweenItem.TargetType => null;

        void IScriptableTweenItem.AddTween(Sequence sequence, Object _) => AddTween(sequence);

        void AddTween(Sequence sequence);
    }
}