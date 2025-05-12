namespace TSS.Utils.Randoms.Int
{
    [System.Serializable]
    public class RandomIntConfig : IRandomConfig<int>
    {
        public int Min;
        public int MaxExclusive;
        
        public IRandomInstance<int> NewInstance(int seed = 0) => 
            new RandomIntInstance(this, seed);
    }
}