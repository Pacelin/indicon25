using System;

namespace TSS.Extras.AI.BehaviourTrees
{
    public class ConditionNode : NodeBase
    {
        private readonly Func<bool> _predicate;
        public ConditionNode(string name, Func<bool> predicate) : base(name) => _predicate = predicate;

        public override EStatus Process() => _predicate.Invoke() ? EStatus.Success : EStatus.Failure;

        public override void Reset() { }
    }
}