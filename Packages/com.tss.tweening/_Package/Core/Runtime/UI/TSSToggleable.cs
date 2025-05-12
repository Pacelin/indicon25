using R3;
using UnityEngine;

namespace TSS.Core.UI
{
    [AddComponentMenu("TSS/UI/Toggleable")]
    public class TSSToggleable : TSSInteractiveUIElement
    {
        private readonly ReactiveProperty<bool> _value = new(false);

        public ReadOnlyReactiveProperty<bool> Value => _value;
        
        public void SetValue(bool value)
        {
            _value.Value = value;
        }

        public void Toggle()
        {
            SetValue(!_value.CurrentValue);
        }

        protected override void OnClick()
        {
            Toggle();
        }
    }
}