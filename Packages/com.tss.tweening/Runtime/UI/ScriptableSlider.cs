using System;
using JetBrains.Annotations;
using R3;
using TSS.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace TSS.Tweening.UI
{
    [PublicAPI] 
    public class ScriptableSlider : MonoBehaviour, 
        IPointerEnterHandler, IPointerExitHandler,
        IPointerDownHandler, IPointerUpHandler,
        IDragHandler, IInitializePotentialDragHandler
    {
        private enum EState
        {
            Unset,
            Disabled,
            Default,
            Hover,
            Drag,
        }

        private enum EDirection
        {
            LeftToRight,
            RightToLeft,
            TopToBottom,
            BottomToTop
        }
     
        public bool Interactable
        {
            set
            {
                if (_interactable == value)
                    return;
                _interactable = value;
                if (!_interactable)
                {
                    if (_drag)
                    {
                        _onDragFinished.OnNext(Unit.Default);
                        _drag = false;
                    }
                    if (_hover)
                    {
                        _onExit.OnNext(Unit.Default);
                        _hover = false;
                    }
                }
                UpdateState();
            }
        }
        
        public Vector2 Range
        {
            set
            {
                if (_range == value)
                    return;
                _range = value;
                var newValue = Mathf.Clamp(_value, _range.x, _range.y);
                if (_roundToInt)
                    newValue = Mathf.Round(newValue);
                if (Math.Abs(_value - newValue) > 0.00001f)
                {
                    _value = newValue;
                    _onValueChanged.OnNext(_value);
                }
                _onNormalizedValueChanged.OnNext(NormalizedValue);
                UpdateHandle();
            }
            get => _range;
        }

        public float Value
        {
            set
            {
                var newValue = Mathf.Clamp(value, _range.x, _range.y);
                if (_roundToInt)
                    newValue = Mathf.Round(newValue);
                if (Math.Abs(_value - newValue) < 0.00001f)
                    return;
                _value = newValue;
                _onValueChanged.OnNext(_value);
                _onNormalizedValueChanged.OnNext(NormalizedValue);
                UpdateHandle();
            }
            get => _value;
        }

        public float NormalizedValue
        {
            set
            {
                var newValue = Mathf.Lerp(_range.x, _range.y, Mathf.Clamp(value, _range.x, _range.y));
                if (_roundToInt)
                    newValue = Mathf.Round(newValue);
                if (Math.Abs(_value - newValue) < 0.00001f)
                    return;
                _value = newValue;
                _onValueChanged.OnNext(_value);
                _onNormalizedValueChanged.OnNext(NormalizedValue);
                UpdateHandle();
            }
            get => Mathf.InverseLerp(_range.x, _range.y, _value);
        }

        public Observable<float> OnValueChanged => _onValueChanged;

        [SerializeField] private bool _interactable = true;
        [SerializeField] private Vector2 _range = new(0, 1);
        [SerializeField] private float _value;
        [SerializeField] private bool _roundToInt;
        [Space] 
        [SerializeField] private EDirection _direction;
        [SerializeField] private Image _fillImage;
        [SerializeField] private RectTransform _handle;
        [Space]
        [SerializeField] private ScriptableTween _toDefaultTween;
        [SerializeField] private ScriptableTween _toHoverTween;
        [SerializeField] private ScriptableTween _toDragTween;
        [SerializeField] private ScriptableTween _toDisabledTween;

        private RectTransform rectTransform => (RectTransform) transform;
        
        private readonly Subject<Unit> _onDragBegin = new();
        private readonly Subject<Unit> _onDrag = new();
        private readonly Subject<Unit> _onDragFinished = new();
        private readonly Subject<Unit> _onEnter = new();
        private readonly Subject<Unit> _onExit = new();
        private readonly Subject<float> _onValueChanged = new();
        private readonly Subject<float> _onNormalizedValueChanged = new();
        
        private bool _hover;
        private bool _drag;
        private EState _activeState = EState.Unset;
        private Vector2 _offset;

        public Observable<Unit> ObserveDrag() => _onDrag;
        public Observable<Unit> ObserveHover() => _onEnter;

        private void OnValidate()
        {
            if (_range.x > _range.y)
                _range = new Vector2(_range.y, _range.x);
            if (_roundToInt)
                _range = new Vector2(Mathf.Round(_range.x), Mathf.Round(_range.y));
            _value = Mathf.Clamp(_value, _range.x, _range.y);
            if (_roundToInt)
                _value = Mathf.Round(_value);
            
            if (_interactable)
            {
                if (_toDefaultTween)
                {
                    _toDefaultTween.Play();
                    _toDefaultTween.Kill(true);
                }
            }
            else
            {
                if (_toDisabledTween)
                {
                    _toDisabledTween.Play();
                    _toDisabledTween.Kill(true);
                }
            }
            
            UpdateHandle();
        }

        private void Awake()
        {
            if (_interactable)
            {
                if (_toDefaultTween)
                {
                    _toDefaultTween.Play();
                    _toDefaultTween.Complete(true);
                }
            }
            else
            {
                if (_toDisabledTween)
                {
                    _toDisabledTween.Play();
                    _toDisabledTween.Complete(true);
                }
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (!_interactable)
                return;
            _hover = true;
            _onEnter.OnNext(Unit.Default);
            UpdateState();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (!_interactable)
                return;
            _hover = false;
            _onExit.OnNext(Unit.Default);
            UpdateState();
        }
        
        public void OnPointerDown(PointerEventData eventData)
        {
            if (!_interactable)
                return;
            _drag = true;
            _onDragBegin.OnNext(Unit.Default);
            UpdateState();
            
            _offset = Vector2.zero;
            if (_handle != null && RectTransformUtility.RectangleContainsScreenPoint(_handle, eventData.pointerPressRaycast.screenPosition, eventData.enterEventCamera))
            {
                Vector2 localMousePos;
                if (RectTransformUtility.ScreenPointToLocalPointInRectangle(_handle, eventData.pointerPressRaycast.screenPosition, eventData.pressEventCamera, out localMousePos))
                    _offset = localMousePos;
            }
            else
            {
                UpdateDrag(eventData, eventData.pressEventCamera);
            }
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (!_interactable)
                return;
            _drag = false;
            _onDragFinished.OnNext(Unit.Default);
            UpdateState();
        }
        
        public void OnDrag(PointerEventData eventData)
        {
            if (!_interactable)
                return;
            _onDrag.OnNext(Unit.Default);
            UpdateDrag(eventData, eventData.pressEventCamera);
        }
        
        public void OnInitializePotentialDrag(PointerEventData eventData) => eventData.useDragThreshold = false;

        private void UpdateState()
        {
            KillTransitionTweens();
            var currentState = EState.Default;
            var tween = _toDefaultTween;

            if (!_interactable)
            {
                currentState = EState.Disabled;
                tween = _toDisabledTween;
            }
            else if (_drag)
            {
                currentState = EState.Drag;
                tween = _toDragTween;
            }
            else if (_hover)
            {
                currentState = EState.Hover;
                tween = _toHoverTween;
            }

            if (currentState != _activeState)
            {
                _activeState = currentState;
                tween.Play();
                if (!gameObject.activeInHierarchy)
                    tween.Complete(true);
            }
        }

        private void UpdateHandle()
        {
            int axis = _direction is EDirection.LeftToRight or EDirection.RightToLeft ? 0 : 1;
            bool reverse = _direction is EDirection.RightToLeft or EDirection.TopToBottom;
            
            if (_fillImage != null)
            {
                Vector2 anchorMin = Vector2.zero;
                Vector2 anchorMax = Vector2.one;

                if (_fillImage != null && _fillImage.type == Image.Type.Filled)
                {
                    _fillImage.fillAmount = NormalizedValue;
                }
                else
                {
                    if (reverse)
                        anchorMin[axis] = 1 - NormalizedValue;
                    else
                        anchorMax[axis] = NormalizedValue;
                }

                _fillImage.rectTransform.anchorMin = anchorMin;
                _fillImage.rectTransform.anchorMax = anchorMax;
            }

            if (_handle != null)
            {
                Vector2 anchorMin = Vector2.zero;
                Vector2 anchorMax = Vector2.one;
                
                anchorMin[axis] = anchorMax[axis] = (reverse ? (1 - NormalizedValue) : NormalizedValue);
                _handle.anchorMin = anchorMin;
                _handle.anchorMax = anchorMax;
            }
        }

        private void UpdateDrag(PointerEventData eventData, Camera cam)
        {
            int axis = _direction is EDirection.LeftToRight or EDirection.RightToLeft ? 0 : 1;
            bool reverse = _direction is EDirection.RightToLeft or EDirection.TopToBottom;

            Vector2 position = eventData.position;

            Vector2 localCursor;
            if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, position, cam, out localCursor))
                return;
            localCursor -= rectTransform.rect.position;

            float val = Mathf.Clamp01((localCursor - _offset)[axis] / rectTransform.rect.size[axis]);
            NormalizedValue = (reverse ? 1f - val : val);
        }
        
        private void KillTransitionTweens()
        {
            if (_toDisabledTween.IsPlaying)
                _toDisabledTween.Kill();
            if (_toDefaultTween.IsPlaying)
                _toDefaultTween.Kill();
            if (_toDragTween.IsPlaying)
                _toDragTween.Kill();
            if (_toHoverTween.IsPlaying)
                _toHoverTween.Kill();
        }
    }
}