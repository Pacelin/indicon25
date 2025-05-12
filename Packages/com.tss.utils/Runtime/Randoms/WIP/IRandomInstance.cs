namespace TSS.Utils.Randoms
{
    public interface IRandomInstance<out T>
    {
        void Reset(int seed = 0);
        T Take();
    }
}