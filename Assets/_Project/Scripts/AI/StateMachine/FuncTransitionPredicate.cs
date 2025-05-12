using System;

namespace TSS.Extras.AI.StateMachines
{
    public class FuncTransitionPredicate : ITransitionPredicate
    {
        private readonly Func<bool> _func;
        
        public FuncTransitionPredicate(Func<bool> func) => _func = func;
        
        public bool Evaluate() => _func.Invoke();
        
        public static implicit operator FuncTransitionPredicate(Func<bool> func) => new(func);
    }
}