using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TSS.Utils.Randoms.Int
{
    [System.Serializable] 
    public class RandomIntNegentropyArrayConfig : IRandomConfig<int>
    {
        [System.Serializable] 
        public struct Entry
        {
            public int Value;
            public float Weight;
        }

        public List<Entry> Entries;
        public bool ResetByTimer;
        public float ResetSeconds;

        public IRandomInstance<int> NewInstance(int seed = 0) => new RandomIntNegentropyInstance(this, seed);
    }
    
    public class RandomIntNegentropyInstance : RandomInstance<int, RandomIntNegentropyArrayConfig>
    {
        private bool _reset;
        private float _takeExpirationTime;
        private float _entropy;
        
        public RandomIntNegentropyInstance(RandomIntNegentropyArrayConfig config, int seed) : base(config, seed)
        {
        }

        public override int Take()
        {
            if (_reset || (Config.ResetByTimer && Time.time > _takeExpirationTime))
            {
                _entropy = Random.NextFloat(0, Config.Entries.Sum(e => e.Weight));
            }
            else
            {
                _entropy += Config.Entries.Min(e => e.Weight);
                var weightSum = Config.Entries.Sum(e => e.Weight);
                if (_entropy > weightSum)
                    _entropy -= weightSum;
            }

            _reset = false;
            _takeExpirationTime = Time.time + Config.ResetSeconds;
            return Config.Entries.GetAccumulatedItem(e => e.Weight, _entropy).Value;
        }

        protected override void OnReset() => _reset = true;
    }
}