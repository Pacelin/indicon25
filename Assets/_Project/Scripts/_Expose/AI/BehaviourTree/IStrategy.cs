namespace TSS.Extras.AI.BehaviourTrees
{
    public interface IStrategy
    {
        NodeBase.EStatus Process();
        void Reset();
    }
}