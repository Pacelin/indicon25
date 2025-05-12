using System;

namespace TSS.Utils.Randoms.Float
{
    [Serializable]
    public class RandomFloatConfig : IRandomConfig<float>
    {
        public float Min = 0;
        public float Max = 1;

        public IRandomInstance<float> NewInstance(int seed = 0) =>
            new RandomFloatInstance(this, seed);
    }
}