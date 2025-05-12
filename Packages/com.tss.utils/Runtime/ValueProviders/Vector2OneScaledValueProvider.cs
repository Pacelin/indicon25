using UnityEngine;

namespace TSS.Utils
{
    [System.Serializable] 
    public class Vector2OneScaledValueProvider : IVector2ValueProvider
    {
        [SerializeField] private float _value;

        public Vector2OneScaledValueProvider(float value) =>
            _value = value;
        
        public Vector2 Get() => Vector2.one * _value;
    }
}