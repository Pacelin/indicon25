using System.Collections.Generic;

namespace TSS.Extras.AI.BehaviourTrees
{
    public class Node : NodeBase
    {
        protected int CurrentChild;
        protected IReadOnlyList<NodeBase> Children => _children;

        private readonly List<NodeBase> _children;

        public Node(string name) : base(name) => _children = new();

        public void AddChild(NodeBase child) => _children.Add(child);

        public override EStatus Process() => _children[CurrentChild].Process();

        public override void Reset()
        {
            CurrentChild = 0;
            foreach (var child in _children)
                child.Reset();
        }
    }
}