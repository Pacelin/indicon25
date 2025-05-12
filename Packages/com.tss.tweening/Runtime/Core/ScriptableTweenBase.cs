using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace TSS.Tweening
{
    public abstract class ScriptableTweenBase : MonoBehaviour
    {
        internal IReadOnlyList<IScriptableTweenItem> Items => _preset ? _preset.Items : _items;
        internal IReadOnlyList<Object> Targets => _targets;
        internal bool CacheTween => _cacheTween;
        internal bool PlayOnEnable => _playOnEnable;
        internal int Loops => _loops;
        internal LoopType LoopType => _loopType;
        internal float Delay => _delay;
        internal bool CompleteOnKill => _completeOnKill;

        [SerializeField] private bool _completeOnKill = false;
        [SerializeField] private ScriptableTweenPreset _preset;
        [SerializeField] private Object[] _targets;
        [SerializeReference] private IScriptableTweenItem[] _items;
        [SerializeField] private bool _cacheTween;
        [SerializeField] private bool _playOnEnable;
        [SerializeField] private int _loops;
        [SerializeField] private LoopType _loopType = LoopType.Restart;
        [SerializeField] private float _delay;
    }
}