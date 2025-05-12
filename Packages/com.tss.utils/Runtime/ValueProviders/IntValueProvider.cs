using UnityEngine;

namespace TSS.Utils
{
    [System.Serializable]
    public class IntValueProvider : IIntValueProvider
    {
        [SerializeField] private int _value;

        public IntValueProvider(int value) => _value = value;
        
        public int Get() => _value;
    }
}