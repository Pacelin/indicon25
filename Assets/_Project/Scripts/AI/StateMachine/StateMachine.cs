using System.Collections.Generic;

namespace TSS.Extras.AI.StateMachines
{
    public class StateMachine
    {
        private readonly Dictionary<IState, StateNode> _states = new();
        private readonly List<Transition> _anyTransitions = new();

        private StateNode _activeState;
        
        public void Run(IState state)
        {
            _activeState = GetOrCreateStateNode(state);
            _activeState.State.OnEnter();
        }

        public void Stop()
        {
            _activeState?.State.OnExit();
            _activeState = null;
        }

        public void Update()
        {
            IState switchingState = GetSwitchingState();
            if (switchingState != null)
                SwitchState(switchingState);
            
            _activeState?.State.OnUpdate();
        }

        public void FixedUpdate() => _activeState?.State.OnFixedUpdate();

        public void AddTransition(IState from, IState to, ITransitionPredicate condition) =>
            GetOrCreateStateNode(from).Transitions.Add(new Transition(GetOrCreateStateNode(to).State, condition));
        public void AddAnyTransition(IState to, ITransitionPredicate condition) =>
            _anyTransitions.Add(new Transition(GetOrCreateStateNode(to).State, condition));

        private IState GetSwitchingState()
        {
            foreach (var anyTransition in _anyTransitions)
                if (anyTransition.Condition.Evaluate())
                    return anyTransition.To;
            if (_activeState == null)
                return null;
            
            foreach (var stateTransition in _activeState.Transitions)
                if (stateTransition.Condition.Evaluate())
                    return stateTransition.To;
            return null;
        }
        
        private void SwitchState(IState state)
        {
            _activeState?.State.OnExit();
            _activeState = _states[state];
            _activeState?.State.OnEnter();
        }
        
        private StateNode GetOrCreateStateNode(IState state)
        {
            if (!_states.ContainsKey(state))
                _states[state] = new StateNode(state);
            return _states[state];
        }
    }
}