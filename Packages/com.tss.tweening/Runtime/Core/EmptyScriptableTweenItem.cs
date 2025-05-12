using System;
using DG.Tweening;

namespace TSS.Tweening
{
    [Serializable]
    [ScriptableTweenPath("None", -1)]
    [NoFoldout]
    public sealed class EmptyScriptableTweenItem : IScriptableTweenItemNoTarget
    {
        public void AddTween(Sequence sequence) { }
    }
}