using System;
using R3;
using TSS.Tweening.UI;
using UnityEngine;

namespace Siberian25.UI
{
    [RequireComponent(typeof(ScriptableSlider))]
    public abstract class SliderVolumeHandlerBase : MonoBehaviour
    {
        [HideInInspector] [SerializeField] private ScriptableSlider _target;

        private IDisposable _disposable;
        
#if UNITY_EDITOR
        private void Reset() => _target = GetComponent<ScriptableSlider>();
        private void OnValidate() => _target = GetComponent<ScriptableSlider>();
#endif

        private void OnEnable()
        {
            _target.Value = GetVolume();
            _disposable = _target.OnValueChanged.Subscribe(SetVolume);
        }

        private void OnDisable() => _disposable?.Dispose();

        protected abstract float GetVolume();
        protected abstract void SetVolume(float volume);
    }
}