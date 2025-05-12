using R3;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace TSS.Core.UI
{
    [AddComponentMenu("TSS/UI/Interactable")]
    public sealed class TSSInteractable : UIBehaviour
    {
        private readonly List<CanvasGroup> _canvasGroupCache = new();
        private bool _groupsAllowInteraction = true;
        
        [SerializeField]
        private bool _interactable = true;

        public bool InteractableSelf
        {
            get => _interactable;
            set
            {
                _interactable = value;
                InteractableSelfChanged?.OnNext(_interactable);
            }
        }

        public bool InteractableInHierarchy => _interactable && _groupsAllowInteraction;
        
        public Subject<bool> InteractableSelfChanged { get; } = new();
        public Subject<bool> InteractableInHierarchyChanged { get; } = new();
        
        protected override void OnCanvasGroupChanged()
        {
            bool parentGroupAllowsInteraction = ParentGroupAllowsInteraction();

            if (parentGroupAllowsInteraction == _groupsAllowInteraction) return;

            _groupsAllowInteraction = parentGroupAllowsInteraction;

            if (_interactable)
                InteractableInHierarchyChanged.OnNext(InteractableInHierarchy);
        }

        private bool ParentGroupAllowsInteraction()
        {
            Transform t = transform;
            while (t != null)
            {
                t.GetComponents(_canvasGroupCache);
                foreach (CanvasGroup canvasGroup in _canvasGroupCache)
                {
                    if (canvasGroup.enabled && !canvasGroup.interactable)
                        return false;

                    if (canvasGroup.ignoreParentGroups)
                        return true;
                }

                t = t.parent;
            }

            return true;
        }
    }
}