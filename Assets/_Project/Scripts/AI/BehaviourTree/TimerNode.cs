using UnityEngine;

namespace TSS.Extras.AI.BehaviourTrees
{
    public class TimerNode : NodeBase
    {
        public enum EUpdateTime
        {
            Update,
            FixedUpdate,
            UnscaledUpdate,
            UnscaledFixedUpdate,
        }

        private readonly EUpdateTime _updateTime;
        private readonly float _duration;
        private float _countdown;
        
        public TimerNode(string name, float duration, EUpdateTime updateTime) : base(name)
        {
            _duration = duration;
            _updateTime = updateTime;
            _countdown = duration;
        }

        public override EStatus Process()
        {
            switch (_updateTime)
            {
                case EUpdateTime.Update:
                    _countdown -= Time.deltaTime;
                    break;
                case EUpdateTime.FixedUpdate:
                    _countdown -= Time.fixedDeltaTime;
                    break;
                case EUpdateTime.UnscaledUpdate:
                    _countdown -= Time.unscaledDeltaTime;
                    break;
                case EUpdateTime.UnscaledFixedUpdate:
                    _countdown -= Time.fixedUnscaledDeltaTime;
                    break;
            }

            if (_countdown > 0)
                return EStatus.Pending;

            Reset();
            return EStatus.Success;
        }

        public override void Reset() => _countdown = _duration;
    }
}