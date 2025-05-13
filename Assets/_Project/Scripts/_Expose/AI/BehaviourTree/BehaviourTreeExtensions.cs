using System;

namespace TSS.Extras.AI.BehaviourTrees
{
    public static class BehaviourTreeExtensions
    {
        public static T WithPriority<T>(this T node, int priority) where T : NodeBase
        {
            node.PriorityFunc = () => priority;
            return node;
        }

        public static T WithPriority<T>(this T node, Func<int> priorityFunc) where T : NodeBase
        {
            node.PriorityFunc = priorityFunc;
            return node;
        }
    }
}