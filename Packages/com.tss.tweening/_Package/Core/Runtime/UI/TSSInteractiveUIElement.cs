using R3;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace TSS.Core.UI
{
    public abstract class TSSInteractiveUIElement : 
        UIBehaviour,
        IPointerClickHandler,
        IPointerDownHandler,
        IPointerUpHandler,
        IPointerEnterHandler,
        IPointerExitHandler
    {
        [SerializeField]
        private TSSInteractable _interactableComponent;

        protected bool InteractableInHierarchy => _interactableComponent && _interactableComponent.InteractableInHierarchy;
        protected bool InteractableSelf => _interactableComponent && _interactableComponent.InteractableSelf;

        private IDisposable _disposable;
        
        protected override void Awake()
        {
            if (!_interactableComponent)
            {
                Debug.LogError($"{nameof(_interactableComponent)} is not assigned.");
                return;
            }

            _disposable = _interactableComponent.InteractableInHierarchyChanged.Subscribe(OnInteractableChanged);
        }

        protected override void OnDestroy()
        {
            _disposable?.Dispose();
        }

        protected virtual void OnInteractableChanged(bool interactableInHierarchy) { }
        
        protected virtual void OnClick() { }

        protected virtual void OnHoverEnter() { }
        
        protected virtual void OnHoverExit() { }
        
        protected virtual void OnPointerDown() { }
        
        protected virtual void OnPointerUp() { }
        
        void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
        {
            if (InteractableInHierarchy)
                OnClick();
        }

        void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
        {
            if (InteractableInHierarchy)
                OnPointerDown();
        }

        void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
        {
            if (InteractableInHierarchy)
                OnPointerUp();
        }

        void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
        {
            if (InteractableInHierarchy)
                OnHoverEnter();
        }

        void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
        {
            if (InteractableInHierarchy)
                OnHoverExit();
        }
    }
}