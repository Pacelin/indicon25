namespace TSS.Utils.Randoms
{
    public interface IChanceInstance : IRandomInstance<bool>
    {
        bool IRandomInstance<bool>.Take() => Success();
        bool Success();
    }
}