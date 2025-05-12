using UnityEngine;

namespace TSS.Utils
{
    [System.Serializable]
    public class FloatRandomBetweenValueProvider : IFloatValueProvider
    {
        [SerializeField] private float _min;
        [SerializeField] private float _max;

        public FloatRandomBetweenValueProvider(float min, float max)
        {
            _min = min;
            _max = max;
        }
        
        public float Get() => Random.Range(_min, _max);
    }
}