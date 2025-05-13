namespace TSS.Extras.Timers
{
    public class FuncBufferEvaluator : ITimeBufferEvaluator
    {
        public bool IsActive => _activeFunc.Invoke();

        private readonly System.Func<bool> _activeFunc;
        
        public FuncBufferEvaluator(System.Func<bool> activeFunc) => _activeFunc = activeFunc;
    }
}