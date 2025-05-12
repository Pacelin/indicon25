using UnityEngine;

namespace TSS.Utils
{
    [System.Serializable]
    public class Vector3ValueProvider : IVector3ValueProvider
    {
        [SerializeField] private Vector3 _value;

        public Vector3ValueProvider(Vector3 value) => _value = value;
        public Vector3ValueProvider() => _value = Vector3.zero;
        public Vector3 Get() => _value;
    }
}