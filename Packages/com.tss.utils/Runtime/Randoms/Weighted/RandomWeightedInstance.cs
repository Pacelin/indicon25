using System.Linq;
using JetBrains.Annotations;

namespace TSS.Utils.Randoms.Weighted
{
    [PublicAPI]
    public class RandomWeightedInstance<T>
    {
        private System.Random _random;
        private readonly WeightedEntry<T>[] _entries;

        public RandomWeightedInstance(WeightedEntry<T>[] entries)
        {
            _entries = entries;
            Reset();
        }

        public void Reset()
        {
            _random = new System.Random();
            
        }

        public T Next()
        {
            var rng = _random.NextFloat(0, _entries.Sum(e => e.Weight));
            return _entries.GetAccumulatedItem(e => e.Weight, rng).Value;
        }
    }
}