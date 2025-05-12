using UnityEngine;

namespace TSS.Utils
{
    [System.Serializable]
    public class Vector2ValueProvider : IVector2ValueProvider
    {
        [SerializeField] private Vector2 _value;

        public Vector2ValueProvider(Vector2 value) => _value = value;
        public Vector2ValueProvider() { }
        public Vector2 Get() => _value;
    }
}