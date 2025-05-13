using UnityEngine;

namespace TSS.Extras.Timers
{
    public class TimestampTimer
    {
        public bool Finished => (_ignoreTimeScale ? Time.unscaledTime : Time.time) >= _estimation;
        public bool Ticking => (_ignoreTimeScale ? Time.unscaledTime : Time.time) < _estimation;
        
        private readonly float _duration;
        private readonly bool _ignoreTimeScale;

        private float _estimation;
        
        public TimestampTimer(float duration, bool ignoreTimeScale)
        {
            _duration = duration;
            _ignoreTimeScale = ignoreTimeScale;
        }

        public void Restart() =>
            _estimation = (_ignoreTimeScale ? Time.unscaledTime : Time.time) + _duration;
    }
}