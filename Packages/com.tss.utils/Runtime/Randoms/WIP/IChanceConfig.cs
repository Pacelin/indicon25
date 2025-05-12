namespace TSS.Utils.Randoms
{
    public interface IChanceConfig : IRandomConfig<bool>
    {
        IRandomInstance<bool> IRandomConfig<bool>.NewInstance(int seed) => NewInstance(seed);
        new IChanceInstance NewInstance(int seed = 0);
    }
}