using R3;
using UnityEngine;

namespace TSS.Core.UI
{
    [AddComponentMenu("TSS/UI/Hoverable")]
    public class TSSHoverable : TSSInteractiveUIElement
    {
        private readonly ReactiveProperty<bool> _hoverState = new(false);

        public ReadOnlyReactiveProperty<bool> HoverState => _hoverState;
        
        protected override void OnInteractableChanged(bool interactable)
        {
            if (!interactable)
            {
                _hoverState.Value = false;
            }
        }

        protected override void OnHoverEnter()
        {
            _hoverState.Value = true;
        }

        protected override void OnHoverExit()
        {
            _hoverState.Value = false;
        }
    }
}