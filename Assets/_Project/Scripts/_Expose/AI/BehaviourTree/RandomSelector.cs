using TSS.Utils;

namespace TSS.Extras.AI.BehaviourTrees
{
    public class RandomSelector : Node
    {
        private NodeBase[] _shuffledChildren;
        
        public RandomSelector(string name) : base(name)
        {
        }

        public override EStatus Process()
        {
            if (_shuffledChildren == null)
                _shuffledChildren = Children.GetShuffled();
            if (CurrentChild < _shuffledChildren.Length)
            {
                switch (_shuffledChildren[CurrentChild].Process())
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
            _shuffledChildren = null;
            base.Reset();
        }
    }
}