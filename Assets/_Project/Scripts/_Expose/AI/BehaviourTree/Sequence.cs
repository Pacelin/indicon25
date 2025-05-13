namespace TSS.Extras.AI.BehaviourTrees
{
    public class Sequence : Node
    {
        public Sequence(string name) : base(name)
        {
        }

        public sealed override EStatus Process()
        {
            if (CurrentChild < Children.Count)
            {
                switch (Children[CurrentChild].Process())
                {
                    case EStatus.Pending:
                        return EStatus.Pending;
                    case EStatus.Failure:
                        Reset();
                        return EStatus.Failure;
                    case EStatus.Success:
                        CurrentChild++;
                        return CurrentChild == Children.Count ? EStatus.Success : EStatus.Pending;
                }
            }
            
            Reset();
            return EStatus.Success;
        }
    }
}