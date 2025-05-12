namespace TSS.Utils.Randoms
{
    public interface IRandomConfig<out T>
    {
        IRandomInstance<T> NewInstance(int seed = 0);
    }
}
