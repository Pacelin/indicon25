namespace TSS.Extras.AI.BehaviourTrees
{
    public class BehaviourNode : NodeBase
    {
        private readonly IStrategy _strategy;
        
        public BehaviourNode(string name, IStrategy strategy) : base(name) => _strategy = strategy;

        public sealed override EStatus Process() => _strategy.Process();
        public sealed override void Reset() => _strategy.Reset();
    }
}