using UnityEngine;

namespace TSS.Utils
{
    [System.Serializable]
    public class IntRandomBetweenValueProvider : IIntValueProvider
    {
        [SerializeField] private int _min;
        [SerializeField] private int _max;

        public IntRandomBetweenValueProvider(int min, int max)
        {
            _min = min;
            _max = max;
        }
        
        public int Get() => Random.Range(_min, _max + 1);
    }
}