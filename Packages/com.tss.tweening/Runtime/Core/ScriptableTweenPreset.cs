using System.Collections.Generic;
using UnityEngine;

namespace TSS.Tweening
{
    [CreateAssetMenu(menuName = "TSS/Scriptable Tween Preset", fileName = "SO_TweenPreset")]
    internal class ScriptableTweenPreset : ScriptableObject
    {
        public IReadOnlyList<IScriptableTweenItem> Items => _items;
        
        [SerializeReference] private IScriptableTweenItem[] _items;
    }
}