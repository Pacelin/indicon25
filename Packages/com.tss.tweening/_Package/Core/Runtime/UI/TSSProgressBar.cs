using DG.Tweening;
using R3;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace TSS.Core.UI
{
    [AddComponentMenu("TSS/UI/Progress Bar")]
    public class TSSProgressBar : UIBehaviour
    {
        [SerializeField]
        private Image _fillMain, _fillTrailDecrease, _fillTrailIncrease;

        [SerializeField]
        private float _minValue, _maxValue;

        [SerializeField, HideInInspector]
        private float _currentTrailValue;
        [SerializeField]
        private float _currentValue;

        [SerializeField]
        private float _trailDelay, _trailTime;

        public float CurrentFillValue => _currentValue;
        public float CurrentTrailValue => _currentTrailValue;

        public Subject<float> FillValueChanged { get; } = new();
        
        private Tween _tween;

        protected override void Awake()
        {
            _currentTrailValue = _currentValue;
        }

        protected override void OnDestroy()
        {
            _tween?.Kill();
        }

        public void SetValue(in float value)
        {
            _currentValue = Mathf.Clamp(value, _minValue, _maxValue);

            float delta = _currentTrailValue - _currentValue;
            float trailDuration = Mathf.Abs(delta / (_maxValue - _minValue) * _trailTime);
            
            Image fill = delta > 0f ? _fillMain : _fillTrailIncrease;
            Image trail = delta > 0f ? _fillTrailDecrease : _fillMain;
            Image otherTrail = delta > 0f ? _fillTrailIncrease : _fillTrailDecrease; 
            
            _tween?.Kill();
            _tween = DOTween
                     .Sequence()
                     .AppendInterval(_trailDelay)
                     .Append(DOVirtual.Float(_currentTrailValue, _currentValue, trailDuration, value => _currentTrailValue = value))
                     .Join(trail.DOFillAmount(Mathf.InverseLerp(_minValue, _maxValue, _currentValue), trailDuration))
                     .SetUpdate(true);

            fill.fillAmount = Mathf.InverseLerp(_minValue, _maxValue, _currentValue);
            otherTrail.fillAmount = fill.fillAmount;
            
            FillValueChanged.OnNext(value);
        }

        #if UNITY_EDITOR
        protected override void OnValidate()
        {
            if (Application.isPlaying)
                return;
            
            _fillMain.fillAmount = Mathf.InverseLerp(_minValue, _maxValue, _currentValue);
            _fillTrailDecrease.fillAmount = _fillMain.fillAmount;
            _fillTrailIncrease.fillAmount = _fillMain.fillAmount;
        }
        #endif
    }
}
