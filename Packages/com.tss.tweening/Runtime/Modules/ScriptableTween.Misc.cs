using System;
using DG.Tweening;
using TSS.Utils;
using UnityEngine;
using UnityEngine.Events;

namespace TSS.Tweening
{
    [Serializable]
    [ScriptableTweenPath("Misc/Nested Tween", 1001)]
    [NotPreset]
    public class ScriptableTweenTween : IScriptableTweenItemNoTarget
    {
        [SerializeField] private ETweenConnectBehaviour _connectBehaviour;
        [SerializeField] private ScriptableTween _tween;

        public void AddTween(Sequence sequence)
        {
            if (_connectBehaviour == ETweenConnectBehaviour.Append)
                sequence.Append(_tween.GetNewTween());
            else
                sequence.Join(_tween.GetNewTween());
        }
    }
    
    [Serializable]
    [ScriptableTweenPath("Misc/Interval", 1001)]
    public class ScriptableTweenInterval : IScriptableTweenItemNoTarget
    {
        [SerializeReference] private IFloatValueProvider _interval;

        public void AddTween(Sequence sequence) =>
            sequence.AppendInterval(_interval.Get());
    }
    
    [Serializable]
    [ScriptableTweenPath("Misc/Enable GO", 1003)]
    [NoFoldout]
    public class ScriptableTweenEnableGO : IScriptableTweenItem<GameObject>
    {
        [SerializeField] private ETweenConnectBehaviour _connectBehaviour;

        public void AddTween(Sequence sequence, GameObject obj)
        {
            if (_connectBehaviour == ETweenConnectBehaviour.Append)
                sequence.AppendCallback(() => obj.SetActive(true));
            else
                sequence.JoinCallback(() => obj.SetActive(true));
        }
    }
    
    [Serializable]
    [ScriptableTweenPath("Misc/Disable GO", 1004)]
    [NoFoldout]
    public class ScriptableTweenDisableGO : IScriptableTweenItem<GameObject>
    {
        [SerializeField] private ETweenConnectBehaviour _connectBehaviour;

        public void AddTween(Sequence sequence, GameObject obj)
        {
            if (_connectBehaviour == ETweenConnectBehaviour.Append)
                sequence.AppendCallback(() => obj.SetActive(false));
            else
                sequence.JoinCallback(() => obj.SetActive(false));
        }
    }
    
    [Serializable]
    [ScriptableTweenPath("Misc/Callback", 1005)]
    [NotPreset]
    public class ScriptableTweenCallback : IScriptableTweenItemNoTarget
    {
        [SerializeField] private ETweenConnectBehaviour _connectBehaviour;
        [SerializeField] private UnityEvent _callback;

        public void AddTween(Sequence sequence)
        {
            if (_connectBehaviour == ETweenConnectBehaviour.Append)
                sequence.AppendCallback(() => _callback.Invoke());
            else
                sequence.JoinCallback(() => _callback.Invoke());
        }
    }
    
    [Serializable]
    [ScriptableTweenPath("Misc/Event", 1006)]
    [NoFoldout]
    public class ScriptableTweenEvent : IScriptableTweenItem<ScriptableTweenEventHandler>
    {
        [SerializeField] private ETweenConnectBehaviour _connectBehaviour;

        public void AddTween(Sequence sequence, ScriptableTweenEventHandler obj)
        {
            if (_connectBehaviour == ETweenConnectBehaviour.Append)
                sequence.AppendCallback(obj.OnTrigger);
            else
                sequence.JoinCallback(obj.OnTrigger);
        }
    }
    
    [Serializable]
    [ScriptableTweenPath("Misc/Cycling Animation", 1007)]
    [NotPreset]
    public class ScriptableTweenCyclingAnimation : IScriptableTweenItemNoTarget
    {
        [SerializeField] private ScriptableTween[] _animations;
        [SerializeField] private float _interval;

        public void AddTween(Sequence sequence)
        {
            for (int i = 0; i < _animations.Length; i++)
            {
                var index = i;
                sequence.AppendCallback(() => _animations[index].Play());
                sequence.AppendInterval(_interval);
            }
        }
    }
}