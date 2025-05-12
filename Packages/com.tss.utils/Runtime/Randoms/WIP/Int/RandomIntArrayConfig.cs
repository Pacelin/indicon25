using System.Collections.Generic;

namespace TSS.Utils.Randoms.Int
{
    [System.Serializable]
    public class RandomIntArrayConfig : IRandomConfig<int>
    {
        public List<int> Entries;

        public IRandomInstance<int> NewInstance(int seed = 0) => new RandomIntArrayInstance(this, seed);
    }

    public class RandomIntArrayInstance : RandomInstance<int, RandomIntArrayConfig>
    {
        public RandomIntArrayInstance(RandomIntArrayConfig config, int seed) : base(config, seed) { }

        public override int Take() => Config.Entries[Random.Next(0, Config.Entries.Count)];
        protected override void OnReset() { }
    }
}