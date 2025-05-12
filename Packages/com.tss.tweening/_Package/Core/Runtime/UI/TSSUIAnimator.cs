using Cysharp.Threading.Tasks;
using AYellowpaper.SerializedCollections;
using UnityEngine;

namespace TSS.Core.UI
{
    public enum ETransitionType
    {
        Default,
        Disabled,
        Hover,
        Down,
    }
    
    [AddComponentMenu("TSS/UI/TSS UI Animator")]
    public class TSSUIAnimator : TSSInteractiveUIElement
    {
        [SerializeField]
        [SerializedDictionary("State", "Animation")]
        private SerializedDictionary<ETransitionType, TSSTweener> _transitions;

        private ETransitionType _currentTransition;
        
        private bool _hovered;
        private bool _clicked;
        
        protected override void OnInteractableChanged(bool interactableInHierarchy)
        {
            PlayTransition(InteractableSelf ? (_hovered ? ETransitionType.Hover : ETransitionType.Default) : ETransitionType.Disabled);
        }

        protected override void OnPointerDown()
        {
            _clicked = true;
            PlayTransition(ETransitionType.Down);
        }

        protected override void OnPointerUp()
        {
            _clicked = false;
            PlayTransition(_hovered ? ETransitionType.Hover : ETransitionType.Default);
        }

        protected override void OnHoverEnter()
        {
            _hovered = true;
            PlayTransition(ETransitionType.Hover);
        }

        protected override void OnHoverExit()
        {
            _hovered = false;
            
            if (!_clicked)
                PlayTransition(ETransitionType.Default);
        }

        private void PlayTransition(ETransitionType type)
        {
            if (_transitions.ContainsKey(_currentTransition))
                _transitions[_currentTransition]?.Kill();
            _currentTransition = type;
            
            if (_transitions.ContainsKey(_currentTransition))
                _transitions[_currentTransition]?.PlayAsync().Forget();
        }
    }
}