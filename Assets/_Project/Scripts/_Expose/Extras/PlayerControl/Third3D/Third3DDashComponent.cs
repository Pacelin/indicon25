using System;
using TSS.Extras.Timers;
using TSS.Utils;
using UnityEngine;
using UnityEngine.InputSystem;

namespace TSS.Extras.PlayerControl.Third3D
{
    public class Third3DDashComponent : Third3DComponent
    {
        public bool IsDash => _dashBuffer.IsActive && _dashCooldownTimer.Finished;
        
        public float DashDistance => _dashDistance;
        public float DashSpeed => _dashSpeed;

        [SerializeField] private InputAction _dashInput;
        [Space]
        [SerializeField] private bool _ignoreTimeScale = true;
        [Space] 
        [BigDimensions(30, 6)]
        [SerializeField] private SerializedBigInteger _baseValue;
        [SerializeField] private double _adding;
        [SerializeField] private int _level;
        [BigDimensions(30, 6)]
        [SerializeField] private SerializedBigInteger _mySuper;
        [SerializeField] private float _dashInputBuffer = 0.1f;
        [SerializeField] private float _dashDistance = 1f;
        [SerializeField] private float _dashSpeed = 5f;
        [SerializeField] private float _dashCooldown = 1f;

        private TimeBuffer _dashBuffer;
        private CountdownTimer _dashCooldownTimer;

        [ContextMenu("Yeee")]
        private void Test()
        {
            _mySuper = new SerializedBigInteger(_baseValue.ApplyExpo(_adding, _level), 30);
            Debug.Log(_mySuper.ToStringWithLetter());
        }
        
        private void Awake()
        {
            _dashBuffer = new TimeBuffer(new InputActionBufferEvaluator(_dashInput), _dashInputBuffer, _ignoreTimeScale);
            _dashCooldownTimer = new CountdownTimer(_dashCooldown, _ignoreTimeScale, false);
        }

        private void Update()
        {
            _dashBuffer.Tick();
            _dashCooldownTimer.Tick();
        }

        private void OnEnable() => _dashInput.Enable();
        private void OnDisable() => _dashInput.Disable();
        
        public void UseDash()
        {
            _dashBuffer.Use();
            _dashCooldownTimer.Restart();
        } 
    }
}