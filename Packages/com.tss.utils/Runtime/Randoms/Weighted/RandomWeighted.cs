using UnityEngine;

namespace TSS.Utils.Randoms.Weighted
{
    [System.Serializable]
    public class RandomWeighted<T>
    {
        [SerializeField] private WeightedEntry<T>[] _entries;

        public RandomWeightedInstance<T> CreateInstance() => new(_entries);
    }
}