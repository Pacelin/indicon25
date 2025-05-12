namespace TSS.Extras.AI.BehaviourTrees
{
    public class BehaviourTree : Node
    {
        public BehaviourTree(string name) : base(name)
        {
        }

        public sealed override EStatus Process()
        {
            while (CurrentChild < Children.Count)
            {
                var status = Children[CurrentChild].Process();
                if (status != EStatus.Success)
                    return status;
                CurrentChild++;
            }

            return EStatus.Success;
        }
    }
}