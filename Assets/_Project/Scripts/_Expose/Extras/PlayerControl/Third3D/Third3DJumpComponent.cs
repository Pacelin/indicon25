using TSS.Extras.Timers;
using UnityEngine;
using UnityEngine.InputSystem;

namespace TSS.Extras.PlayerControl.Third3D
{
    public class Third3DJumpComponent : Third3DComponent
    {
        public bool IsJump => _jumpBuffer.IsActive;

        public float JumpForce => _jumpForce;

        [SerializeField] private InputAction _jumpInput;
        [Space]
        [SerializeField] private bool _ignoreTimeScale = true;
        [Space]
        [SerializeField] private float _jumpInputBuffer = 0.1f;
        [SerializeField] private float _jumpForce = 3f;

        private TimeBuffer _jumpBuffer;

        private void Awake() => _jumpBuffer = 
            new TimeBuffer(new InputActionBufferEvaluator(_jumpInput), _jumpInputBuffer, _ignoreTimeScale);

        private void Update() => _jumpBuffer.Tick();

        private void OnEnable() => _jumpInput.Enable();
        private void OnDisable() => _jumpInput.Disable();
        
        public void UseJump() => _jumpBuffer.Use();
    }
}