using System;

namespace TSS.Extras.AI.StateMachines
{
    public interface ITransitionPredicate
    {
        bool Evaluate();
    }
}