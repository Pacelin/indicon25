using UnityEngine;

namespace TSS.Utils.Randoms.ADHOC
{
    [System.Serializable]
    public class RandomADHOC<T>
    {
        [SerializeField] private ADHOCEntry<T>[] _entries;

        public RandomADHOCInstance<T> CreateInstance() => new RandomADHOCInstance<T>(_entries);
    }
}