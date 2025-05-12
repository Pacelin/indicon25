namespace TSS.Utils
{
    public interface IValueProvider<out T>
    {
        T Get();
    }
}