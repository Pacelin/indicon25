using UnityEngine;
using UnityEngine.InputSystem;

namespace TSS.Extras.PlayerControl.Third3D
{
    public class Third3DWalkComponent : Third3DComponent
    {
        public bool IsMove => _moveInput.IsPressed();
        public Vector2 MoveDirection => _moveInput.ReadValue<Vector2>();
        public float WalkSpeed => _walkSpeed;
        
        [SerializeField] private InputAction _moveInput;
        [SerializeField] private float _walkSpeed;

        private void OnEnable() => _moveInput.Enable();
        private void OnDisable() => _moveInput.Disable();
    }
}