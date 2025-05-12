using UnityEngine;

namespace TSS.Utils
{
    [System.Serializable]
    public class FloatValueProvider : IFloatValueProvider
    {
        [SerializeField] private float _value;

        public FloatValueProvider(float value) => _value = value;
        
        public float Get() => _value;
    }
}