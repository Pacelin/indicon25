using System.Linq;

namespace TSS.Extras.AI.BehaviourTrees
{
    public class PrioritySelector : Node
    {
        private NodeBase[] _sortedChildren;
        
        public PrioritySelector(string name) : base(name)
        {
        }

        public override EStatus Process()
        {
            if (_sortedChildren == null)
                _sortedChildren = Children.OrderByDescending(child => child.Priority).ToArray();
            if (CurrentChild < _sortedChildren.Length)
            {
                switch (_sortedChildren[CurrentChild].Process())
                {
                    case EStatus.Pending:
                        return EStatus.Pending;
                    case EStatus.Success:
                        Reset();
                        return EStatus.Success;
                    case EStatus.Failure:
                        CurrentChild++;
                        return EStatus.Pending;
                }
            }

            Reset();
            return EStatus.Failure;
        }

        public override void Reset()
        {
            _sortedChildren = null;
            base.Reset();
        }
    }
}