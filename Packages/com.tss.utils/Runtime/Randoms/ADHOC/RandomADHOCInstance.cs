using System.Linq;
using JetBrains.Annotations;

namespace TSS.Utils.Randoms.ADHOC
{
    [PublicAPI]
    public class RandomADHOCInstance<T>
    {
        private System.Random _random;
        private readonly ADHOCEntry<T>[] _entries;
        private readonly float[] _wheights;

        public RandomADHOCInstance(ADHOCEntry<T>[] entries)
        {
            _entries = entries;
            _wheights = new float[entries.Length];
            Reset();
        }

        public void Reset()
        {
            _random = new System.Random();
            for (int i = 0; i < _entries.Length; i++)
                _wheights[i] = _entries[i].Weight;
        }

        public T Next()
        {
            var index = _wheights.GetAccumlatedItemIndex(_random.NextFloat(0, _wheights.Sum()));
            for (int i = 0; i < index; i++)
                _wheights[i] += _entries[i].WeightIncreaseOnFail;
            _wheights[index] = _entries[index].Weight;
            for (int i = index + 1; i < _wheights.Length; i++)
                _wheights[i] += _entries[i].WeightIncreaseOnFail;
            return _entries[index].Value;
        }
    }
}