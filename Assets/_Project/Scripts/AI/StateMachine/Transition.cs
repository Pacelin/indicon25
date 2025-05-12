namespace TSS.Extras.AI.StateMachines
{
    public class Transition
    {
        public IState To { get; }
        public ITransitionPredicate Condition { get; }

        public Transition(IState to, ITransitionPredicate condition)
        {
            To = to;
            Condition = condition;
        }
    }
}