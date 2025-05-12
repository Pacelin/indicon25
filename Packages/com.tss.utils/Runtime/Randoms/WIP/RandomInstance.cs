using System;

namespace TSS.Utils.Randoms
{
    public abstract class RandomInstance<T, TConfig> : IRandomInstance<T>
        where TConfig : IRandomConfig<T>
    {
        protected Random Random => _random;
        protected TConfig Config { get; }

        private Random _random;

        protected RandomInstance(TConfig config, int seed)
        {
            Config = config;
            Reset(seed);
        } 

        public void Reset(int seed = 0)
        {
            _random = new Random(seed == 0 ? (int) DateTime.Now.Ticks : seed);
            OnReset();
        }

        public abstract T Take();
        protected abstract void OnReset();
    }
}