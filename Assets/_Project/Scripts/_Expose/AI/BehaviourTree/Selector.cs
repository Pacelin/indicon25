namespace TSS.Extras.AI.BehaviourTrees
{
    public class Selector : Node
    {
        public Selector(string name) : base(name)
        {
        }

        public override EStatus Process()
        {
            if (CurrentChild < Children.Count)
            {
                switch (Children[CurrentChild].Process())
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
    }
}