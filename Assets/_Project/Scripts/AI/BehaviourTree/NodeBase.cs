using System;

namespace TSS.Extras.AI.BehaviourTrees
{
    public abstract class NodeBase
    {
        public enum EStatus { Success, Failure, Pending }

        public string Name { get; }
        
        public int Priority => PriorityFunc?.Invoke() ?? 0;
        internal Func<int> PriorityFunc { get; set; }
        
        protected NodeBase(string name)
        {
            Name = name;
        }
        
        public abstract EStatus Process();
        public abstract void Reset();
    }
}