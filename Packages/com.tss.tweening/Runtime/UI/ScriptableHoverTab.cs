using JetBrains.Annotations;
using R3;
using TSS.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

namespace TSS.Tweening.UI
{
    [PublicAPI]
    public class ScriptableHoverTab : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        private enum EState
        {
            Unset,
            Disabled,
            Default,
            Hover
        }
        
        public bool Interactable
        {
            get => _interactable;
            set
            {
                if (_interactable == value)
                    return;
                _interactable = value;
                if (!_interactable)
                {
                    if (_hover)
                    {
                        _onExit.OnNext(Unit.Default);
                        _hover = false;
                    }
                }
                UpdateState();
            }
        }
        
        [SerializeField] private bool _interactable = true;
        [SerializeField] private ScriptableTween _toDefaultTween;
        [SerializeField] private ScriptableTween _toHoverTween;
        [SerializeField] private ScriptableTween _toDisabledTween;
        
        private bool _hover;
        private EState _activeState = EState.Unset;
        
        private readonly Subject<Unit> _onEnter = new();
        private readonly Subject<Unit> _onExit = new();

        public Observable<Unit> ObserveExit() => _onExit;
        public Observable<Unit> ObserveEnter() => _onEnter;

        private void OnValidate()
        {
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
        }
        
        private void Awake()
        {
            if (_interactable)
            {
                _toDefaultTween.Play();
                _toDefaultTween.Complete(true);
                _activeState = EState.Default;
            }
            else
            {
                _toDisabledTween.Play();
                _toDisabledTween.Complete(true);
                _activeState = EState.Disabled;
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

        private void KillTransitionTweens()
        {
            if (_toDisabledTween.IsPlaying)
                _toDisabledTween.Kill();
            if (_toDefaultTween.IsPlaying)
                _toDefaultTween.Kill();
            if (_toHoverTween.IsPlaying)
                _toHoverTween.Kill();
        }
    }
}