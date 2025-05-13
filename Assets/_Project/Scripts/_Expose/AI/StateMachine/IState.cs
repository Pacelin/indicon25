namespace TSS.Extras.AI.StateMachines
{
    public interface IState
    {
        void OnEnter();
        void OnExit();
        void OnUpdate();
        void OnFixedUpdate();
    }
}
