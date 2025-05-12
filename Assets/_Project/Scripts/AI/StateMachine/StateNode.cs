using System.Collections.Generic;

namespace TSS.Extras.AI.StateMachines
{
    public class StateNode
    {
        public IState State => _state;
        public IList<Transition> Transitions => _transitions;
        
        private readonly IState _state;
        private readonly List<Transition> _transitions;
        
        public StateNode(IState state)
        {
            _state = state;
            _transitions = new();
        }
    }
}