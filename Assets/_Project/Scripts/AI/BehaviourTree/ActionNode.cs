using System;

namespace TSS.Extras.AI.BehaviourTrees
{
    public class ActionNode : NodeBase
    {
        private readonly Action _action;
        public ActionNode(string name, Action action) : base(name) => _action = action;

        public override EStatus Process()
        {
            _action.Invoke();
            return EStatus.Success;
        }

        public override void Reset() { }
    }
}