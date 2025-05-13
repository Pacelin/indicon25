using UnityEngine;
using UnityEngine.InputSystem;

namespace TSS.Extras.PlayerControl.Third3D
{
    public class Third3DRunComponent : Third3DComponent
    {
        public bool IsRun => _runInput.IsPressed();
        public float RunSpeed => _runSpeed;
        
        [SerializeField] private InputAction _runInput;
        [SerializeField] private float _runSpeed;
    
        private void OnEnable() => _runInput.Enable();
        private void OnDisable() => _runInput.Disable();
    }
}