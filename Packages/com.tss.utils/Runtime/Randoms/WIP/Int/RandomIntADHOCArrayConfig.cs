using System.Collections.Generic;
using System.Linq;

namespace TSS.Utils.Randoms.Int
{
    [System.Serializable] 
    public class RandomIntADHOCArrayConfig : IRandomConfig<int>
    {
        [System.Serializable] 
        public struct Entry
        {
            public int Value;
            public float Weight;
            public float WeightIncreaseOnFail;
        }

        public List<Entry> Entries;

        public IRandomInstance<int> NewInstance(int seed = 0) => new RandomIntADHOCArrayInstance(this, seed);
    }

    public class RandomIntADHOCArrayInstance : RandomInstance<int, RandomIntADHOCArrayConfig>
    {
        private float[] _wheights;
        
        public RandomIntADHOCArrayInstance(RandomIntADHOCArrayConfig config, int seed) : base(config, seed)
        {
        }

        public override int Take()
        {
            var index = _wheights.GetAccumlatedItemIndex(Random.NextFloat(0, _wheights.Sum()));
            for (int i = 0; i < index; i++)
                _wheights[i] += Config.Entries[i].WeightIncreaseOnFail;
            _wheights[index] = Config.Entries[index].Weight;
            for (int i = index + 1; i < _wheights.Length; i++)
                _wheights[i] += Config.Entries[i].WeightIncreaseOnFail;
            return Config.Entries[index].Value;
        }

        protected override void OnReset()
        {
            _wheights = new float[Config.Entries.Count];
            for (int i = 0; i < _wheights.Length; i++)
                _wheights[i] = Config.Entries[i].Weight;
        }
    }
}