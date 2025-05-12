using R3;
using UnityEngine;
using UnityEngine.EventSystems;

namespace TSS.Core.UI
{
    [AddComponentMenu("TSS/UI/Option Button")]
    public class TSSOptionButton : UIBehaviour
    {
        [SerializeField]
        private TSSClickable _previousOption;
        [SerializeField]
        private TSSClickable _nextOption;

        public Subject<Unit> PreviousOptionClicked => _previousOption.Clicked;
        public Subject<Unit> NextOptionClicked => _nextOption.Clicked;
    }
}