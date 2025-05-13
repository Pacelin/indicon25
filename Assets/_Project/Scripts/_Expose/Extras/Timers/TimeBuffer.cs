namespace TSS.Extras.Timers
{
    public class TimeBuffer
    {
        public bool IsActive => !_used && _buffer.Ticking;

        private bool _used;

        private readonly ITimeBufferEvaluator _evaluator;
        private readonly TimestampTimer _buffer;

        public TimeBuffer(ITimeBufferEvaluator evaluator, float bufferDuration, bool ignoreTimescale)
        {
            _evaluator = evaluator;
            _buffer = new TimestampTimer(bufferDuration, ignoreTimescale);
        }

        public void Use() => _used = true;
        
        public void Tick()
        {
            if (_evaluator.IsActive)
                _buffer.Restart();
            else
                _used = false;
        }
    }
}