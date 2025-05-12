using FMODUnity;
using R3;
using UnityEngine;

namespace TSS.Core.UI
{
    [AddComponentMenu("TSS/UI/Clickable")]
    public class TSSClickable : TSSInteractiveUIElement
    {
        public Subject<Unit> Clicked { get; } = new();

        [SerializeField] private bool _sfx = false;
        [SerializeField] private EventReference _event;

        protected override void OnClick()
        {
            if (_sfx)
                RuntimeManager.PlayOneShot(_event);
            Clicked.OnNext(Unit.Default);
        }
    }
}