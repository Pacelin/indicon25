using System;
using TSS.Tweening.UI;
using R3;
using UnityEngine;

namespace TSS.Tweening.UI
{
    [RequireComponent(typeof(ScriptableSlider))]
    public abstract class SliderHandlerBase : MonoBehaviour
    {
        [HideInInspector] [SerializeField] private ScriptableSlider _target;

        private IDisposable _disposable;
        
#if UNITY_EDITOR
        private void Reset() => _target = GetComponent<ScriptableSlider>();
        private void OnValidate() => _target = GetComponent<ScriptableSlider>();
#endif

        private void OnEnable()
        {
            _disposable = Disposable.Combine(
                _target.ObserveDrag().Subscribe(_ => OnDrag()),
                _target.ObserveHover().Subscribe(_ => OnHover())
            );
        }

        private void OnDisable() => _disposable?.Dispose();

        protected abstract void OnDrag();
        protected abstract void OnHover();
    }
}