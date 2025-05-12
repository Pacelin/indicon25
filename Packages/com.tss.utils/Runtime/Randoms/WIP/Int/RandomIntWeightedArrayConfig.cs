using System.Collections.Generic;
using System.Linq;

namespace TSS.Utils.Randoms.Int
{
    [System.Serializable] 
    public class RandomIntWeightedArrayConfig : IRandomConfig<int>
    {
        [System.Serializable] 
        public struct Entry
        {
            public int Value;
            public float Weight;
        }

        public List<Entry> Entries;

        public IRandomInstance<int> NewInstance(int seed = 0) => new RandomIntWeightedInstance(this, seed);
    }
    
    public class RandomIntWeightedInstance : RandomInstance<int, RandomIntWeightedArrayConfig>
    {
        public RandomIntWeightedInstance(RandomIntWeightedArrayConfig config, int seed) : base(config, seed)
        {
        }

        public override int Take() =>
            Config.Entries.GetAccumulatedItem(e => e.Weight, 
                    Random.NextFloat(0, Config.Entries.Sum(en => en.Weight))).Value;

        protected override void OnReset()
        {
        }
    }
}