using UnityEngine.InputSystem;

namespace TSS.Extras.Timers
{
    public class InputActionBufferEvaluator : ITimeBufferEvaluator
    {
        public bool IsActive => _action.IsPressed();
        private readonly InputAction _action;
        public InputActionBufferEvaluator(InputAction action) => _action = action;
    }
}