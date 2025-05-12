using System;
using TSS.Tweening.UI;
using R3;
using UnityEngine;

namespace TSS.Tweening.UI
{
    [RequireComponent(typeof(ScriptableButton))]
    public abstract class ButtonHandlerBase : MonoBehaviour
    {
        [HideInInspector] [SerializeField] private ScriptableButton _target;

        private IDisposable _disposable;
        
        #if UNITY_EDITOR
        private void Reset() => _target = GetComponent<ScriptableButton>();
        private void OnValidate() => _target = GetComponent<ScriptableButton>();
        #endif

        private void OnEnable()
        {
            _disposable = Disposable.Combine(
                _target.ObserveClick().Subscribe(_ => OnClick()),
                _target.ObserveEnter().Subscribe(_ => OnHover())
            );
        }

        private void OnDisable() => _disposable?.Dispose();

        protected abstract void OnClick();
        protected abstract void OnHover();
    }
}