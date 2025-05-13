using TSS.Core;
using UnityEngine;

namespace TSS.Extras.Timers
{
    public class CountdownTimer
    {
        public bool Finished => _countdown <= 0;
        public bool Ticking => _countdown > 0;
        
        private readonly float _duration;
        private readonly bool _ignoreTimeScale;
        private readonly bool _ignorePause;

        private float _countdown;
        
        public CountdownTimer(float duration, bool ignoreTimeScale, bool ignorePause)
        {
            _duration = duration;
            _ignoreTimeScale = ignoreTimeScale;
            _ignorePause = ignorePause;
        }

        public void Restart() => _countdown = _duration;

        public void Tick()
        {
            if (!_ignorePause && Runtime.IsPaused)
                return;

            _countdown -= _ignoreTimeScale ? Time.unscaledDeltaTime : Time.deltaTime;
        }
    }
}